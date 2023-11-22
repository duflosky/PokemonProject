using System;
using Manager;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Fight
{
    public class UICapacityButton : MonoBehaviour, ISelectHandler
    {
        private UIFight uiParent;
        [SerializeField] private TextMeshProUGUI text;
        private Button button;
        private Image image;
        private int index;

        private void Start()
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
        }

        public void InitButton(UIFight parent, int _index)
        {
            uiParent = parent;
            index = _index;
        }
        
        public void SetCapacity(CapacityInstance _capacity)
        {
            text.text = _capacity.so.name;
            button.enabled = image.enabled = true;
        }
        
        public void SetEmpty()
        {
            text.text = "_";
            button.enabled = image.enabled = false;
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            uiParent.DisplayCapacity(index);
        }

        public void ButtonAction()
        {
            FightManager.Instance.ProcessTurn(index);
        }

        
    }
}