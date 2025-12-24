using System;
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
        public void OnEnqueue()
        {
        }

        public void OnDequeue()
        {
        }
    }
}