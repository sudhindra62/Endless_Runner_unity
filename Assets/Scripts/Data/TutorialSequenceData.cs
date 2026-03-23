
using UnityEngine;
using System.Collections.Generic;

    /// <summary>
    /// A ScriptableObject that defines a sequence of tutorial steps.
    /// </summary>
    [CreateAssetMenu(fileName = "TutorialSequence", menuName = "Endless Runner/Tutorial Sequence")]
    public class TutorialSequenceData : ScriptableObject
    {
        [Tooltip("The list of steps that make up this tutorial sequence.")]
        public List<TutorialStep> steps = new List<TutorialStep>();
    }

