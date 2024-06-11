using HintSystem.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace HintSystem
{
    public static class Hint
    {
        public class HintInfo
        {
            public HintInfo(string hint, string completionKey)
            {
                this.hint = hint;
                this.completionKey = completionKey;
            }

            public string hint;
            public string completionKey;
        }

        private static Queue<HintInfo> hintQueue = new Queue<HintInfo>();
        private static HintInfo activeHint;


        private static HintOutput Output
        {
            get
            {
                if (output == null)
                {
                    output = Object.FindObjectOfType<HintOutput>();
                }

                return output;
            }
        }
        private static HintOutput output;

        public static void ShowHint(HintInfo newHint)
        {
            hintQueue.Enqueue(newHint);
            TryHintDequeue();
        }

        public static void ShowHint(string hintContent, string completionKey)
        {
            ShowHint(new HintInfo(hintContent, completionKey));
        }

        public static void SendCompletionKey(string completionKey)
        {
            if (activeHint == null)
            {
                if (activeHint.completionKey == completionKey)
                {
                    Output.HideHint();
                    activeHint = null;
                    TryHintDequeue();
                }
            }
        }

        public static void ClearHintQueue()
        {
            hintQueue.Clear();
            Output?.HideHint();
            activeHint = null;
        }

        private static void TryHintDequeue()
        {
            if (activeHint == null && hintQueue.Count > 0)
            {
                activeHint = hintQueue.Dequeue();
                Output.ShowHint(activeHint.hint);
            }
        }
    }

}