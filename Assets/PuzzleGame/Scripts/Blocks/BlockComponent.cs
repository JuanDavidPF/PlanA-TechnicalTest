using System;
using PlanA.Architecture.Architecture.ObjectPool;
using PlanA.Architecture.DataBinding;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.Blocks
{
    [Serializable]
    public struct BlockData
    {
        public float BlockTypeHue;
        public int BlockType;
        public Vector2Int Position;
    }

    public class BlockDataBind : DataBind<BlockDataBind, BlockData>
    {
        public BlockDataBind(BlockData value = default) : base(value)
        {
        }
    }
}