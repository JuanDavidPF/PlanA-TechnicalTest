using PlanA.Architecture.Architecture.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.Blocks
{
    [RequireComponent(typeof(Image))]
    public sealed class SlotController : MonoBehaviour, IPoolable
    {
        public void OnEnqueue()
        {
        }

        public void OnDequeue()
        {
        }
    }
}