using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(fileName = "NewTutorialSequence", menuName = "Tutorial/Tutorial Sequence")]
    public class TutorialSequence : ScriptableObject
    {
        public List<TutorialStep> steps;
        public bool canSkip = true;
        public int stepToSkipTo;
    }
}
