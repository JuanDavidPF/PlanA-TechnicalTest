using Cysharp.Threading.Tasks;
using PlanA.Architecture.Architecture.ObjectPool;
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

        private void OnBlockDataChanged(BlockDataBind blockDataBind)
        {
            Button.image.sprite = spriteVariant[blockDataBind.Value.BlockType % spriteVariant.Length];
        }

        private void OnTapped()
        {
        }

        public UniTask SpawnAnimation()
        {
            return UniTask.CompletedTask;
        }

        public UniTask DespawnAnimation()
        {
            return UniTask.CompletedTask;
        }

        public void SetWidth(float width)
        {
            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }

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

        public bool AreSameType(BlockController other)
        {
            return BlockData.Value.BlockType == other.BlockData.Value.BlockType;
        }
    }
}