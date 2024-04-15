// Author: Chrysalis shiyuchongf@gmail.com

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ProjectHKU.UI;

namespace ProjectHKU.UI
{
    public class Popup : UIElement
    {
        public Text desc;

        public void SetText(string targetDesc)
        {
            desc.text = targetDesc;
        }
    }
}
