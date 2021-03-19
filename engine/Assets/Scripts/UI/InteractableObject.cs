using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Synthesis.UI.ContextMenus;
using ContextMenu = Synthesis.UI.ContextMenus.ContextMenu;

namespace Synthesis.UI
{
    public class InteractableObject : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        // public class ContextItemEvent : UnityEvent { }
        public string ContextMenuUID;
        public List<(string title, Action callback)> Options = new List<(string title, Action callback)>();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //open right click floating window
                Debug.Log(eventData.position);
                
                Vector2 position = new Vector2(eventData.position.x, eventData.position.y - 1080); // Maybe have that 1080 number adjust but for rn it's fine
                ContextMenu.Show(position, ContextMenuUID, Options);
            } else {
                ContextMenu.Hide();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //open tooltips and highlight on
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //close tooltips and highlight off
        }

        public void AddOption(string title, Action callback) => Options.Add((title, callback));
    }
}
