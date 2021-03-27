using System.Net.NetworkInformation;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Synthesis.Attributes;
using TMPro;

#nullable enable

namespace Synthesis.UI.Hierarchy.HierarchyItems
{
    public class HierarchyItem : InteractableObject
    {
        public delegate void OnClickEvent();
        public event OnClickEvent OnItemClicked;

        #region Properties

        public bool IsInherited {
            get => this.GetType() != typeof(HierarchyItem);
        }

        public int Index {
            get {
                if (Parent == null)
                    return 0;
                return Parent.GetIndex(this);
            }
        }
        public int LocalIndex {
            get {
                if (Parent == null)
                    return 0;
                return Parent.GetLocalIndex(this);
            }
        }
        public int GlobalIndex {
            get {
                if (Parent == null)
                    return 0;
                return Parent.GetGlobalIndex(this);
            }
        }
        public int Depth {
            get {
                if (Parent == null)
                    return 0;
                return Parent.Depth + 1;
            }
        }
        public TMP_Text TitleText;
        // public Transform Root;
        public HierarchyFolderItem? Parent;
        private string title = String.Empty;
        public string Title {
            get => title;
            set {
                title = value;
                TitleText.text = title;
                gameObject.name = title;
                base.ContextMenuUID = title;
            }
        }
        private bool visible = true;
        public bool Visible {
            get => visible;
            set {
                visible = value;
                SetVisible(visible);
            }
        }

        #endregion

        // Use this for initialization
        protected void Awake() {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                OnItemClicked?.Invoke();
            });
        }

        #region Hierarchy

        public virtual void Init(string title, HierarchyFolderItem? parent) {
            Title = title;
            if (parent != null) {
                // Parent = parent;
                parent.Add(this);
            }
            Visible = true;
            // if (Parent != null)
            //     Root = Parent.Root;
        }

        protected virtual void SetVisible(bool visible) {
            gameObject.SetActive(visible);
        }

        public virtual void Remove() {
            HierarchyFolderItem parent = Parent!;
            while (parent != null) {
                int num = parent.Items.RemoveAll(x => x.item.gameObject == this.gameObject);
                // Debug.Log($"{Title} removed {num} times");
                parent = parent.Parent!;
            }
            Parent = null;
            Visible = false;
            Hierarchy.Changes = true;
        }

        #endregion

        #region ContextMenu

        [ContextMenuOption("Remove", "CloseIcon")]
        public void RemoveContextMenu() {
            Remove();
        }

        #endregion
    }
}