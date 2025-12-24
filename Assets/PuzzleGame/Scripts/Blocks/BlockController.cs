using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlanA.Architecture.Architecture.ObjectPool;
using PlanA.Architecture.DataBinding;
using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.Blocks
{
    [RequireComponent(typeof(Button))]
    public sealed class BlockController : MonoBehaviour, IPoolable
    {
        [SerializeField] public Sprite[] spriteVariant;

        [field: SerializeField] public Button Button { get; private set; }

        [field: SerializeReference] public BlockDataBind BlockData { private set; get; } = new();

        /// <summary>
        /// Called when the block is returned to the pool.
        /// Cleans up listeners and data bindings.
        /// </summary>
        public void OnEnqueue()
        {
            Button.onClick.RemoveListener(OnTapped);
            BlockData.UnBind(OnBlockDataChanged);
        }

        /// <summary>
        /// Called when the block is taken from the pool.
        /// Resets state and rebinds listeners and data.
        /// </summary>
        public void OnDequeue()
        {
            Button.interactable = false;
            gameObject.SetActive(false);
            Button.onClick.AddListener(OnTapped);
            BlockData.Bind(OnBlockDataChanged);
        }

        /// <summary>
        /// Updates the visual representation when block data changes.
        /// </summary>
        private void OnBlockDataChanged(BlockDataBind blockDataBind)
        {
            Button.image.sprite =
                spriteVariant[blockDataBind.Value.BlockType % spriteVariant.Length];
        }

        /// <summary>
        /// Handles user tap interaction and raises the corresponding game event.
        /// </summary>
        private void OnTapped()
        {
            ServiceLocator.Get<EventBusService>()
                .Raise(new OnBlockTapped { Block = this });

            IntDataBind moves = GameManager.Instance.RuntimeGameData.Moves;

            if (moves.Value > 0)
            {
                moves.SetValue(moves.Value - 1);
            }
        }

        /// <summary>
        /// Animates the block moving into its slot.
        /// </summary>
        public async UniTask MoveToSlotAnimation(float duration = 0.25f)
        {
            Button.interactable = false;

            await transform
                .DOLocalMove(Vector3.zero, duration)
                .SetEase(Ease.OutBounce)
                .ToUniTask();

            Button.interactable = true;
        }

        /// <summary>
        /// Animates the block spawning from above its slot.
        /// </summary>
        public async UniTask SpawnFromTopAnimation(float distance = 500f, float duration = 0.35f)
        {
            Button.interactable = false;

            RectTransform rect = (RectTransform)transform;
            rect.localPosition = Vector3.up * distance;
            gameObject.SetActive(true);

            await transform
                .DOLocalMove(Vector3.zero, duration)
                .SetEase(Ease.OutBounce)
                .ToUniTask();

            Button.interactable = true;
        }

        /// <summary>
        /// Animates the block despawning before returning to the pool.
        /// </summary>
        public async UniTask DespawnAnimation(float duration = 1f)
        {
            Button.interactable = false;

            await transform
                .DOScale(Vector3.zero, duration)
                .From(Vector3.one * 1.2f)
                .SetEase(Ease.InOutBounce)
                .ToUniTask();
        }

        /// <summary>
        /// Sets the visual width of the block.
        /// </summary>
        public void SetWidth(float width)
        {
            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta =
                new Vector2(width, rectTransform.sizeDelta.y);
        }

        /// <summary>
        /// Determines whether this block is the same type as another block.
        /// </summary>
        public bool AreSameType(BlockController other)
        {
            return BlockData.Value.BlockType == other.BlockData.Value.BlockType;
        }
    }
}