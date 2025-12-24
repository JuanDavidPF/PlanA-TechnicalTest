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

        public void OnEnqueue()
        {
            Button.onClick.RemoveListener(OnTapped);
            BlockData.UnBind(OnBlockDataChanged);
        }

        public void OnDequeue()
        {
            Button.interactable = false;
            gameObject.SetActive(false);
            Button.onClick.AddListener(OnTapped);
            BlockData.Bind(OnBlockDataChanged);
        }

        private void OnBlockDataChanged(BlockDataBind blockDataBind)
        {
            Button.image.sprite = spriteVariant[blockDataBind.Value.BlockType % spriteVariant.Length];
        }

        private void OnTapped()
        {
            ServiceLocator.Get<EventBusService>().Raise(new OnBlockTapped { Block = this });

            IntDataBind moves = GameManager.Instance.RuntimeGameData.Moves;

            if (moves.Value > 0)
            {
                moves.SetValue(moves.Value - 1);
            }
        }

        public async UniTask MoveToSlotAnimation(float duration = 0.25f)
        {
            Button.interactable = false;
            await transform
                .DOLocalMove(Vector3.zero, duration)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
            Button.interactable = true;
        }

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

        public async UniTask DespawnAnimation(float duration = 1f)
        {
            Button.interactable = false;
            await transform.DOScale(Vector3.zero, duration)
                .From(Vector3.one * 1.2f)
                .SetEase(Ease.InOutBounce)
                .ToUniTask();
        }

        public void SetWidth(float width)
        {
            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }

        public bool AreSameType(BlockController other)
        {
            return BlockData.Value.BlockType == other.BlockData.Value.BlockType;
        }
    }
}