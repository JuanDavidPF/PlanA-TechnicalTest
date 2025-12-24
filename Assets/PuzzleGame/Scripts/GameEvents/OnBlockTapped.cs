using PlanA.Architecture.EventBus;
using PlanA.PuzzleGame.Blocks;
using UnityEngine;

namespace PlanA.PuzzleGame.GameEvents
{
    public struct OnBlockTapped : IGameEvent
    {
        public BlockController Block;
    }
}