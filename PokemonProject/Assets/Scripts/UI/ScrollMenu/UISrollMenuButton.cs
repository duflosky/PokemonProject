using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UISrollMenuButton : MonoBehaviour, ISelectHandler
    {
        public UIScrollMenu parent;
        
        public void OnSelect(BaseEventData eventData)
        {
            parent.SelectButton(gameObject);
        }
    }
}