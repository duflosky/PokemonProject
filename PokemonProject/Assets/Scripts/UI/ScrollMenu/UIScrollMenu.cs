using System;
using Manager;
using UnityEngine;

namespace UI
{
    public class UIScrollMenu : UIMenu
    {
        [SerializeField] private GameObject defaultFisrtSelected;

        private GameObject lastObjectSelected;

        private void Start()
        {
            lastObjectSelected = defaultFisrtSelected;
        }

        public override void InitMenu()
        {
            EventManager.Instance.SelectObject(lastObjectSelected);
        }

        public override void GoBackMenu()
        {
            UIManager.Instance.ReturnMenu();
        }

        public void SelectButton(GameObject go)
        {
            lastObjectSelected = go;
        }

        public void ActiveAction(int index)
        {
            switch (index)
            {
                case 0: break;
                case 1:
                    UIManager.Instance.OpenMenu(Enums.UIMenus.PokemonTeamMenu,true);
                    break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 5: break;
                case 6: break;
                default: throw new NotImplementedException();
            }
        }
    }
}