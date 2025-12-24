using PlanA.Architecture.Services;
using UnityEngine;

namespace PlanA.PuzzleGame
{
    public sealed class AudioDispatcher : IGameService
    {
        readonly private AudioSource _audioSource;

        public AudioDispatcher(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Play(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Initialize()
        {
        }

        public void DeInitialize()
        {
        }
    }
}