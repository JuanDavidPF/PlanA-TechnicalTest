using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlanA.Architecture;
using PlanA.Architecture.Architecture.ObjectPool;
using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.Blocks
{
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class GridController : Singleton<GridController>
    {
        static readonly private Vector2Int[] MatchDirections =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Pools")]
        [SerializeReference] private ObjectPool<SlotController> _slotPool = new();
        [SerializeReference] private ObjectPool<BlockController> _blockPool = new();

        [Header("SFX")]
        [SerializeReference] private AudioClip _blockPop;

        private int _blockTypes;

        private RectTransform _rectTransform;
        private Dictionary<Vector2Int, SlotController> _slots = new();

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = (RectTransform)transform;
            _slotPool.Initialize();
            _blockPool.Initialize();
        }

        private void Start()
        {
            ServiceLocator.Get<EventBusService>().Subscribe<OnGameStarted>(BuildBoard);
        }

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnBlockTapped>(OnBlockTapped);
            }

            _slotPool.Clear();
            _blockPool.Clear();
        }

        private void BuildBoard(OnGameStarted onGameStarted)
        {
            _canvasGroup.interactable = false;
            CleanBoard();

            GameData gameData = GameManager.Instance.RuntimeGameData;
            _blockTypes = gameData.BlockTypes;

            _gridLayoutGroup.cellSize = new Vector2(CalculateBlockSize(gameData.Columns), 112);
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = gameData.Columns;

            _slots = new Dictionary<Vector2Int, SlotController>(gameData.Rows * gameData.Columns);

            List<UniTask> spawnOperations = new();

            for (int row = 0; row < gameData.Rows; row++)
            {
                for (int column = 0; column < gameData.Columns; column++)
                {
                    Vector2Int position = new(column, row);

                    SlotController slotController = _slotPool.Dequeue(_gridLayoutGroup.transform);
                    slotController.transform.localScale = Vector3.one;
                    _slots[position] = slotController;

                    //This delay, creates a wave effect when creating the tiles
                    float waveDelay = (column + row) * 0.05f;
                    UniTask blockSpawn = UniTask.WaitForSeconds(waveDelay).ContinueWith(() => SpawnBlock(position));
                    spawnOperations.Add(blockSpawn);
                }
            }

            UniTask.WhenAll(spawnOperations).ContinueWith(() =>
            {
                EventBusService eventBusService = ServiceLocator.Get<EventBusService>();
                eventBusService.Raise(new OnBoardCreated());
                eventBusService.Subscribe<OnBlockTapped>(OnBlockTapped);
                eventBusService.Subscribe<OnGameOver>(OnGameOver);
                _canvasGroup.interactable = true;
            }).Forget();
        }

        private BlockController SpawnBlock(Vector2Int position)
        {
            SlotController slot = _slots[position];
            if (slot.Block) return slot.Block;

            BlockController block = _blockPool.Dequeue(slot.transform);
            slot.SetBlock(block);

            block.BlockData.SetValue(new BlockData
            {
                BlockType = Random.Range(0, _blockTypes),
                Position = position
            });
            block.SpawnFromTopAnimation().Forget();
            return block;
        }

        private void OnBlockTapped(OnBlockTapped onBlockTapped)
        {
            GameData gameData = GameManager.Instance.RuntimeGameData;
            gameData.Score.SetValue(gameData.Score.Value + gameData.TapPoints);

            Vector2Int position = onBlockTapped.Block.BlockData.Value.Position;
            BlockController block = onBlockTapped.Block;
            block.transform.SetParent(_blockPool.EnqueuedContainer, true);

            SlotController slot = _slots[position];
            slot.RemoveBlock();

            //Despawn tapped tile
            ServiceLocator.Get<AudioDispatcher>().Play(_blockPop);
            onBlockTapped.Block.DespawnAnimation(0.35f).ContinueWith(() => _blockPool.Enqueue(onBlockTapped.Block)).Forget();

            //Recursively yap matching neighbour tiles, with a slight delay.
            UniTask.WaitForSeconds(0.15f).ContinueWith(() => TapMatchingNeighbours(onBlockTapped.Block)).Forget();

            //Collapse the column
            UniTask.WaitForSeconds(0.5f).ContinueWith(() => CollapseColumn(position.x)).Forget();
        }

        private void TapMatchingNeighbours(BlockController block)
        {
            Vector2Int blockPosition = block.BlockData.Value.Position;

            foreach (Vector2Int direction in MatchDirections)
            {
                Vector2Int neighbourPosition = blockPosition + direction;
                if (!_slots.TryGetValue(neighbourPosition, out SlotController slot) || slot.Block == null || !slot.Block.Button.interactable) continue;

                if (block.AreSameType(slot.Block))
                {
                    OnBlockTapped(new OnBlockTapped { Block = slot.Block });
                }
            }
        }

        private void CollapseColumn(int column)
        {
            GameData gameData = GameManager.Instance.RuntimeGameData;

            int writeRow = 0;

            for (int readRow = 0; readRow < gameData.Rows; readRow++)
            {
                Vector2Int readPos = new(column, readRow);
                SlotController readSlot = _slots[readPos];

                if (readSlot.Block == null)
                    continue;

                if (readRow != writeRow)
                {
                    Vector2Int writePos = new(column, writeRow);
                    SlotController writeSlot = _slots[writePos];

                    BlockController block = readSlot.RemoveBlock();
                    writeSlot.SetBlock(block);

                    block.BlockData.SetValue(new BlockData
                    {
                        BlockType = block.BlockData.Value.BlockType,
                        Position = writePos
                    });

                    block.MoveToSlotAnimation().Forget();
                }

                writeRow++;
            }

            for (int row = writeRow; row < gameData.Rows; row++)
            {
                Vector2Int spawnPos = new(column, row);
                SpawnBlock(spawnPos);
            }
        }

        private void OnGameOver(OnGameOver onGameOver)
        {
            _canvasGroup.interactable = false;
            EventBusService eventBusService = ServiceLocator.Get<EventBusService>();
            eventBusService.Unsubscribe<OnBlockTapped>(OnBlockTapped);
            eventBusService.Unsubscribe<OnGameOver>(OnGameOver);
        }

        private void CleanBoard()
        {
            foreach (SlotController slot in _slots.Values)
            {
                BlockController block = slot.RemoveBlock();

                if (block)
                {
                    _blockPool.Enqueue(block);
                }

                _slotPool.Enqueue(slot);
            }

            _slots.Clear();
        }

        private float CalculateBlockSize(int columns)
        {
            float blockSize = _rectTransform.rect.width;
            blockSize -= _gridLayoutGroup.padding.horizontal;
            blockSize -= _gridLayoutGroup.spacing.x * columns - 1;
            return blockSize / columns;
        }
    }
}