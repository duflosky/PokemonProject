using System;
using Manager;
using UnityEngine;

namespace UI.PokemonTeam
{
    public class UIPokemonTeam : UIMenu
    {
        [SerializeField]private UIPokemonDisplayer[] pokemonDisplayers;
        [SerializeField] private GameObject defaultFisrtSelected;

        private Enums.UIPanel currentPanelSelected;
        
        public override void InitMenu()
        {
            for (int i = 0; i < pokemonDisplayers.Length; i++)
            {
                if (GameManager.Instance.team[i] == null)
                {
                    pokemonDisplayers[i].gameObject.SetActive(false);
                    continue;
                }
                pokemonDisplayers[i].InitDisplayer(GameManager.Instance.team[i]);
            }
            EventManager.Instance.SelectObject(defaultFisrtSelected);
        }

        public override void GoBackMenu()
        {
            switch (currentPanelSelected)
            {
                case Enums.UIPanel.None:
                    UIManager.Instance.ReturnMenu();
                    break;
                case Enums.UIPanel.StatisticPanel:
                    CloseStatisticPanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OpenStatisticPanel()
        {
            
        }

        void CloseStatisticPanel()
        {
            
        }
        
        
    }
}