using PlanA.Architecture.EventBus;
using PlanA.PuzzleGame.Blocks;

namespace PlanA.PuzzleGame.GameEvents
{
    public struct OnBlockTapped : IGameEvent
    {
        public BlockController Block;
    }
}