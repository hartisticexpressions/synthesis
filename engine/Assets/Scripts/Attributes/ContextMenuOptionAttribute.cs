using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Attributes {
    public class ContextMenuOptionAttribute : Attribute {

        private string title = string.Empty;
        public string Title { get; private set; }
        private Action callback = null;
        public Action Callback { get; private set; }

        public ContextMenuOptionAttribute() { }

        public ContextMenuOptionAttribute(string title) {
            Title = title;
        }

        public ContextMenuOptionAttribute(string title, Action callback) {
            Title = title;
            Callback = callback;
        }
    }
}
