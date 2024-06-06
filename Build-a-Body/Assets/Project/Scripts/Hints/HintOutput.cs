using TMPro;
using UnityEngine;

namespace HintSystem.Internal
{
    public class HintOutput : MonoBehaviour
    {

        private TextMeshProUGUI hintText;

        private void Awake()
        {
            hintText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnDestroy()
        {
            Hint.ClearHintQueue();
        }

        public void ShowHint(string hintContent)
        {
            hintText.text = hintContent;
        }

        public void HideHint()
        {
            hintText.text = "";
        }

    }

}

