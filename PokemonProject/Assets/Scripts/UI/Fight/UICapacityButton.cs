using SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Fight
{
    public class UICapacityButton : MonoBehaviour, ISelectHandler
    {
        private UIFight uiParent;
        [SerializeField] private TextMeshProUGUI text;
        private int index;

        private CapacityInstance capacity;

        public void InitButton(UIFight parent, int _index)
        {
            uiParent = parent;
            index = _index;
        }
        
        public void SetCapacity(CapacityInstance _capacity)
        {
            capacity = _capacity;
            text.text = capacity.so.name;
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            uiParent.SetCapacity(index);
        }
    }
}