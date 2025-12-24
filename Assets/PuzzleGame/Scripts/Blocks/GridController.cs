using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlanA.Architecture.Architecture.ObjectPool;
using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PlanA.PuzzleGame.Blocks
{
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class GridController : MonoBehaviour
    {
        private void Start()
        {
            ServiceLocator.Get<EventBusService>().Subscribe<OnGameStarted>(BuildBoard);
        }

        private void BuildBoard(OnGameStarted onGameStarted)
        {
        }

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnGameStarted>(BuildBoard);
            }
        }
    }
}