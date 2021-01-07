using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Synthesis.UI
{
    public class InteractableObject : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //open right click floating window
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
    }
}
