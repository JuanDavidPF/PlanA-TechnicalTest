using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame.UI
{
    public sealed class GameOverCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _replayButton;

        private void Start()
        {
            _replayButton.onClick.AddListener(OnReplayButtonClicked);
            ServiceLocator.Get<EventBusService>().Subscribe<OnGameStarted>(OnGameStarted);
            ServiceLocator.Get<EventBusService>().Subscribe<OnGameOver>(OnGameOver);
        }

        private void OnGameStarted(OnGameStarted onGameStarted)
        {
            _canvas.enabled = false;
        }

        private void OnGameOver(OnGameOver onGameOver)
        {
            _canvas.enabled = true;
        }

        static private void OnReplayButtonClicked()
        {
            ServiceLocator.Get<EventBusService>().Raise(new OnReplayRequested());
        }

        private void OnDestroy()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClicked);

            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnGameStarted>(OnGameStarted);
                eventBusService.Unsubscribe<OnGameOver>(OnGameOver);
            }
        }
    }
}