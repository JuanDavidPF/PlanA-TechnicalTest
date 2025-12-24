using System.Globalization;
using PlanA.Architecture.DataBinding;
using TMPro;
using UnityEngine;

namespace PlanA.PuzzleGame.UI
{
    public sealed class MoveCanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _movesText;

        private void Start()
        {
            GameManager.Instance.RuntimeGameData.Moves.Bind(OnScoreUpdated);
        }

        private void OnScoreUpdated(IntDataBind movesBind)
        {
            _movesText.text = movesBind.Value.ToString("N0", CultureInfo.CurrentCulture);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.RuntimeGameData.Moves.UnBind(OnScoreUpdated);
            }
        }
    }
}