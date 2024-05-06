using TMPro;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New DialogueCharacter", menuName = "Dialogue/DialogueCharacter")]
    public class DialogueCharacter : ScriptableObject
    {
        [System.Serializable]
        public struct DialogueCharacterSettings
        {
            public string displayName;
            public Sprite characterSprite;
            public TMP_FontAsset fontAsset;
            public float talkingSpeed;
            public string talkingSound;
            public bool playSoundEachCharacter;
        }

        public DialogueCharacterSettings characterSettings;

    }

}

