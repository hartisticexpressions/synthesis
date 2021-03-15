using System.Net.NetworkInformation;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

#nullable enable

namespace Synthesis.UI.Hierarchy.HierarchyItems
{
    public class HierarchyItem : MonoBehaviour
    {
        public delegate void OnClickEvent();
        public event OnClickEvent OnItemClicked;

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

        // Use this for initialization
        void Awake() {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                OnItemClicked?.Invoke();
            });
        }

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

        /*protected virtual void InitProtected()
        {

        }

        public virtual int GetSize() => 1;

        public void Show() => Show(!gameObject.activeSelf);
        public virtual void Show(bool show) => gameObject.SetActive(show);*/
    }
}