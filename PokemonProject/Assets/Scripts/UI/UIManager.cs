using System;
using System.Collections.Generic;
using UI.Fight;
using UI.PokemonTeam;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Header("Canvas")]
        [SerializeField] private UIScrollMenu uiScrollMenu;
        [SerializeField] private UIPokemonTeam uiPokemonTeam;
        [SerializeField] private UIFight uiFight;

        private UIMenu currentUiSelected;

        private Stack<Enums.UIMenus> menuStack = new();

        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            uiScrollMenu.gameObject.SetActive(false);
            uiFight.gameObject.SetActive(false);
            uiPokemonTeam.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))GoBackMenu();
        }

        void GoBackMenu()
        {
            if(menuStack.Count == 0)
            {
                OpenMenu(Enums.UIMenus.ScrollMenu,true);
                Time.timeScale = 0;
            }
            else currentUiSelected.GoBackMenu();
        }

        public void ReturnMenu()
        {
            CloseMenu(menuStack.Pop());
            if(menuStack.Count == 0 )return;
            OpenMenu(menuStack.Peek());
        }

        public void OpenMenu(Enums.UIMenus ui, bool pushMenu = false)
        {
            if(pushMenu)menuStack.Push(ui);
            if(currentUiSelected)currentUiSelected.gameObject.SetActive(false);
            switch (ui)
            {
                case Enums.UIMenus.ScrollMenu:
                    currentUiSelected = uiScrollMenu;
                    uiScrollMenu.gameObject.SetActive(true);
                    uiScrollMenu.InitMenu();
                    break;
                case Enums.UIMenus.PokemonTeamMenu:
                    currentUiSelected = uiPokemonTeam;
                    uiPokemonTeam.gameObject.SetActive(true);
                    currentUiSelected.InitMenu();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ui), ui, null);
            }
        }
        
        private void CloseMenu(Enums.UIMenus ui)
        {
            switch (ui)
            {
                case Enums.UIMenus.ScrollMenu:
                    uiScrollMenu.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    break;
                case Enums.UIMenus.PokemonTeamMenu:
                    uiPokemonTeam.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ui), ui, null);
            }
            
        }

    }
}