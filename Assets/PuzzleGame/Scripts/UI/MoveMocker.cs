using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlanA.PuzzleGame
{
    public class MoveMocker : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(MakeMove);
        }

        private void MakeMove()
        {
            GameData gameData = GameManager.Instance.RuntimeGameData;
            gameData.Moves.SetValue(gameData.Moves.Value - 1);
            gameData.Score.SetValue(gameData.Score.Value + 10);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(MakeMove);
        }
    }
}