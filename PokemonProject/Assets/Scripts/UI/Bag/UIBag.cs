using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Bag
{
    public class UIBag : UIMenu
    {
        [SerializeField] private Image sectionImage;
        [SerializeField] private Image bagImage;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private UIItemDisplayer[] _itemDisplayers;
        [Space]
        [SerializeField] private Sprite[] sectionSprites;
         [SerializeField] private Sprite[] bagSprites;
        
        private Enums.ObjectType lastSectionOpened = Enums.ObjectType.KeyItems;
        private int lastItemSelect;
        public override void InitMenu()
        {
            OpenSection(lastSectionOpened);
            EventManager.Instance.SelectObject(_itemDisplayers[lastItemSelect].gameObject);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) SwitchSection(-1);
            if(Input.GetKeyDown(KeyCode.RightArrow)) SwitchSection(1);
        }

        void SwitchSection(int move)
        {
            var index = (int)lastSectionOpened + move;
            if(index <0 || index> 2) return;
            OpenSection((Enums.ObjectType)index);
        }
        
        private void OpenSection(Enums.ObjectType objectType)
        {
            lastSectionOpened = objectType;
            EventManager.Instance.SelectObject(_itemDisplayers[0].gameObject);
            
            sectionImage.sprite = sectionSprites[(int)objectType];
            bagImage.sprite = bagSprites[(int)objectType];
            leftArrow.SetActive(objectType != Enums.ObjectType.Items);
            rightArrow.SetActive(objectType != Enums.ObjectType.Pokeballs);
            
            //TODO Fix la nav qui foire au hcangement de panel
            var itemList = GameManager.Instance.inventory[objectType];
            bool quitSet = false;
            for (int i = 0; i < _itemDisplayers.Length; i++)
            {
                _itemDisplayers[i].gameObject.SetActive(true);
                if (i == _itemDisplayers.Length - 1 && !quitSet)
                {
                    _itemDisplayers[i].SetEnd();
                    quitSet = true;
                }

                if (i >= itemList.Count)
                {
                    if (!quitSet)
                    {
                        _itemDisplayers[i].SetEnd();
                        quitSet = true;
                    }
                    else _itemDisplayers[i].gameObject.SetActive(false);
                }
                else _itemDisplayers[i].SetItem(itemList[i]);
            }
        }

        public override void GoBackMenu()
        {
            UIManager.Instance.ReturnMenu();
        }
    }
}