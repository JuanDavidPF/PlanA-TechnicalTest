using PlanA.Architecture.DataBinding;
using TMPro;
using UnityEngine;

namespace PlanA.PuzzleGame.UI
{
    public sealed class ScoreCanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoresText;

        private void Start()
        {
            GameManager.Instance.RuntimeGameData.Score.Bind(OnMovesUpdated);
        }

        private void OnMovesUpdated(IntDataBind scoresBind)
        {
            _scoresText.text = scoresBind.Value.ToString();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.RuntimeGameData.Score.UnBind(OnMovesUpdated);
            }
        }
    }
}