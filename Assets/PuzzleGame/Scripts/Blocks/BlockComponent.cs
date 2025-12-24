using System;
using PlanA.Architecture.DataBinding;
using UnityEngine;

namespace PlanA.PuzzleGame.Blocks
{
    [Serializable]
    public struct BlockData
    {
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