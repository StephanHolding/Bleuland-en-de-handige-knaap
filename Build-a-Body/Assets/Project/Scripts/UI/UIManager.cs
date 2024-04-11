// Author: Chrysalis shiyuchongf@gmail.com

using System;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectHKU.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance = null;
        public static UIManager instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<UIManager>();
                if (_instance == null) throw new Exception("UI not found");
                return _instance;
            }
        }

        [NonSerialized]
        public Dictionary<string, UIElement> ComponentsList = new Dictionary<string, UIElement>();


        // #####################
        // # Localization      #
        // #####################
        [Serializable]
        public struct NamedTextAsset
        {
            public string key;
            public TextAsset value;
        }
        [SerializeField]
        public NamedTextAsset[] localizationFiles;
        [SerializeField]
        public string userLanguage;
        [NonSerialized]
        private Dictionary<string, string> _localizationData = null;
        public Dictionary<string, string> localization
        {
            get
            {
                if (_localizationData == null)
                {
                    TextAsset locFile = localizationFiles[0].value;
                    foreach (var file in localizationFiles)
                    {
                        if (file.key == userLanguage) locFile = file.value;
                    }
                    _localizationData = LocalizationHandler.Load(locFile);
                }
                return _localizationData;
            }
        }

        // temporary
        public void Start()
        {
            if (Application.isEditor) Application.targetFrameRate = 24;
            /*            if (GameManager.instance.currentWinMinigame != "")
                            popup(GameManager.instance.currentWinMinigame + " Finish");
                        else
                            popup("GAME_START");*/
        }

        public void popup(string descID)
        {
            ComponentsList["popup"].gameObject.SetActive(true);
            if (localization.ContainsKey(descID))
                descID = localization[descID];
            else
                Debug.Log($"Localization missing: {descID}");
            ((Popup)ComponentsList["popup"]).SetText(descID);
        }

    }
}
