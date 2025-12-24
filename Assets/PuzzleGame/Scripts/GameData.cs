using PlanA.Architecture.DataBinding;
using UnityEngine;

namespace PlanA.PuzzleGame
{
    [CreateAssetMenu(menuName = "Plan A/PuzzleGame/GameData")]
    public sealed class GameData : ScriptableObject
    {
        [SerializeReference] public IntDataBind Moves = new(5);
        [SerializeReference] public IntDataBind Score = new();
        public int Columns = 5;
        public int Rows = 6;
        public int BlockTypes = 5;
        public int TapPoints = 50;

        public void Assign(GameData gameData)
        {
            if (gameData == null)
            {
                Debug.LogError("GameData.Assign called with null source");
                return;
            }

            Moves.SetValue(gameData.Moves.Value);
            Score.SetValue(gameData.Score.Value);
            Columns = gameData.Columns;
            Rows = gameData.Rows;
            BlockTypes = gameData.BlockTypes;
            TapPoints = gameData.TapPoints;
        }
    }
}