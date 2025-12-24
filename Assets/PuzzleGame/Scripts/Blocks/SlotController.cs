using PlanA.Architecture.Architecture.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.Blocks
{
    [RequireComponent(typeof(Image))]
    public sealed class SlotController : MonoBehaviour, IPoolable
    {
        public BlockController Block { get; private set; }

        public void OnEnqueue()
        {
        }

        public void OnDequeue()
        {
        }

        public void SetBlock(BlockController newBlock)
        {
            newBlock.transform.SetParent(transform, true);
            newBlock.transform.localScale = Vector3.one;
            Block = newBlock;
            Block.SetWidth(((RectTransform)transform).rect.width);
        }

        public BlockController RemoveBlock()
        {
            BlockController blockToRemove = Block;
            Block = null;
            return blockToRemove;
        }
    }
}