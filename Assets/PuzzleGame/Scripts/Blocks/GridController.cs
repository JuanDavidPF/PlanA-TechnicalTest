using System.Collections.Generic;
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
    public sealed class GridController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Pools")]
        [SerializeReference] private ObjectPool<SlotController> _slotPool = new();
        [SerializeReference] private ObjectPool<BlockController> _blockPool = new();

        private RectTransform _rectTransform;
        private Dictionary<Vector2Int, SlotController> _slots = new();

        private int _blockTypes = 0;

        static readonly private Vector2Int[] MatchDirections =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _slotPool.Initialize();
            _blockPool.Initialize();
        }

        private void Start()
        {
            ServiceLocator.Get<EventBusService>().Subscribe<OnGameStarted>(BuildBoard);
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

            for (int row = 0; row < gameData.Rows; row++)
            {
                for (int column = 0; column < gameData.Columns; column++)
                {
                    Vector2Int position = new(column, row);

                    SlotController slotController = _slotPool.Dequeue(_gridLayoutGroup.transform);
                    slotController.transform.localScale = Vector3.one;
                    _slots[position] = slotController;
                }
            }

            EventBusService eventBusService = ServiceLocator.Get<EventBusService>();
            eventBusService.Raise(new OnBoardCreated());
            eventBusService.Subscribe<OnBlockTapped>(OnBlockTapped);
            eventBusService.Subscribe<OnGameOver>(OnGameOver);
            _canvasGroup.interactable = true;
        }

        private void OnBlockTapped(OnBlockTapped onBlockTapped)
        {
        }

        private void TapMatchingNeighbours(BlockController block)
        {
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

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnBlockTapped>(OnBlockTapped);
            }

            _slotPool.Clear();
            _blockPool.Clear();
        }
    }
}