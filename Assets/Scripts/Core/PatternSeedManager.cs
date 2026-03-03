
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Manages the seed for the procedural pattern generator to ensure deterministic runs.
    /// This is critical for features like replays, ghost runs, and fair multiplayer competition.
    /// </summary>
    public class PatternSeedManager : MonoBehaviour
    {
        public int CurrentSeed { get; private set; }
        private System.Random _random;

        /// <summary>
        /// Initializes the seed manager, either with a new random seed or one from session data.
        /// </summary>
        public void Initialize(RunSessionData runSessionData)
        {
            if (runSessionData != null && runSessionData.HasSeed)
            {
                SetSeed(runSessionData.PatternSeed);
            }
            else
            {
                GenerateNewSeed();
                if (runSessionData != null)
                {
                    runSessionData.PatternSeed = CurrentSeed;
                    runSessionData.HasSeed = true;
                }
            }
        }

        private void GenerateNewSeed()
        {
            CurrentSeed = (int)System.DateTime.Now.Ticks;
            _random = new System.Random(CurrentSeed);
            Debug.Log($"PatternSeedManager: New random seed generated: {CurrentSeed}");
        }

        public void SetSeed(int seed)
        {
            CurrentSeed = seed;
            _random = new System.Random(CurrentSeed);
            Debug.Log($"PatternSeedManager: Seed set from session data: {CurrentSeed}");
        }

        public int GetNext(int min, int max)
        {
            return _random.Next(min, max);
        }

        public float GetNextFloat()
        {
            return (float)_random.NextDouble();
        }
    }
}
