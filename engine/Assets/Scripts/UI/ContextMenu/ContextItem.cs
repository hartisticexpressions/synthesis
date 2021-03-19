using System.Net.Http.Headers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Synthesis.UI.ContextMenus {
    public class ContextItem : MonoBehaviour {
        public TMP_Text TextObj;

        private Action callback;
        public Action Callback {
            get => callback;
            set => callback = value;
        }
        public string Text {
            get => TextObj.text;
            set {
                TextObj.text = value;
                gameObject.name = value;
            }
        }

        public void OnClick() {
            ContextMenu.Hide();
            callback();
        }
    }
}
