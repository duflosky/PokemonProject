using System;
using System.Threading.Tasks;
using Manager;
using SO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Fight
{
    public class UIFight : UIMenu
    {
        [SerializeField] private TextMeshProUGUI currentPPText;
        [SerializeField] private TextMeshProUGUI maxPPText;
        [SerializeField] private TextMeshProUGUI attackTypeText;
        [SerializeField] private PokemonInfoBox enemyInfoBox;
        [SerializeField] private PokemonInfoBox allyInfoBox;
        [Space] 
        [SerializeField] private UICapacityButton[] capacitiesButtons;
        [Space]
        [SerializeField] private GameObject fightPanel;
        [SerializeField] private GameObject actionPanel;
        [SerializeField] private GameObject logPanel;
        [Space]
        [SerializeField] private UILogger logger;
        [Header("Event System Selection")] 
        [SerializeField] private Button actionPanelFirstSelect;
        [SerializeField] private Button fightPanelFirstSelect;

         //private PokemonInstance allyInstance;

        private Enums.UIPanel currentSelectedUI;

        void Start()
        {
            for (int i = 0; i < capacitiesButtons.Length; i++)
            {
                capacitiesButtons[i].InitButton(this,i);
            }
        }
        
        public override void InitMenu()
        {
            fightPanel.SetActive(false);
            EventManager.Instance.SelectObject(actionPanelFirstSelect.gameObject);
        }

        public void DisplayPokemonInfo(PokemonInstance pokemon, bool ally)
        {
            if (ally)
            {
                allyInfoBox.InitBox(pokemon);
                SetPokemonCapacities(pokemon);
            }
            else
            {
                enemyInfoBox.InitBox(pokemon);
            }
        }

        void SetPokemonCapacities(PokemonInstance pokemon)
        {
            for (int i = 0; i < 4; i++)
            {
                if (pokemon.capacities[i] == null) capacitiesButtons[i].SetEmpty();
                else capacitiesButtons[i].SetCapacity(pokemon.capacities[i]);
            }
        }

        public void DisplayCapacity(int index)
        {
            var capacity = FightManager.Instance.currentAllyPokemon.capacities[index];
            currentPPText.text = $"{capacity.currentPP}";
            maxPPText.text = $"{capacity.maxPP}";
            attackTypeText.text = $"{capacity.so.attackType}".ToUpper();
        }

        
        public void OpenFightPanel()
        {
            actionPanel.SetActive(false);
            fightPanel.SetActive(true);
            currentSelectedUI = Enums.UIPanel.FightPanel;
            EventManager.Instance.SelectObject(fightPanelFirstSelect.gameObject);
        }
        public void CloseFightPanel()
        {
            fightPanel.SetActive(false);
            actionPanel.SetActive(true);
            currentSelectedUI = Enums.UIPanel.None;
            EventManager.Instance.SelectObject(actionPanelFirstSelect.gameObject);
        }

        public override void GoBackMenu()
        {
            switch (currentSelectedUI)
            {
                case Enums.UIPanel.None:
                    break;
                case Enums.UIPanel.FightPanel:
                    CloseFightPanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task UpdateLife(bool allyAttack)
        {
            var box = allyAttack? enemyInfoBox : allyInfoBox;
            await box.UpdateLife();
        }

        public async Task LogMessage(string message)
        {
           await logger.LogMessage(message);
        }

        public void StartTurn()
        {
            actionPanel.SetActive(false);
            fightPanel.SetActive(false);
        }
        
        public void EndTurn()
        {
            actionPanel.SetActive(true);
            EventManager.Instance.SelectObject(actionPanelFirstSelect.gameObject);
        }
    }
}