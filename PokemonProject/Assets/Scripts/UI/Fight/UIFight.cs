using System;
using Manager;
using SO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Fight
{
    public class UIFight : MonoBehaviour
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
        [Header("Event System Selection")] 
        [SerializeField] private Button actionPanelFirstSelect;
        [SerializeField] private Button fightPanelFirstSelect;

        [Header("DEBUG")] 
        [SerializeField]private PokemonSO allyPokemon;
        [SerializeField]private PokemonSO enemyPokemon;
         private PokemonInstance allyInstance;
        private PokemonInstance enemyInstance;

        private Enums.UISelection currentSelectedUI = Enums.UISelection.ActionMenu;

        private void Start()
        {
            fightPanel.SetActive(false);
            for (int i = 0; i < capacitiesButtons.Length; i++)
            {
                capacitiesButtons[i].InitButton(this,i);
            }
            
            
            allyInstance = new PokemonInstance(allyPokemon, 10);
            allyInstance.currentExp = 150;
            enemyInstance = new PokemonInstance(enemyPokemon, 13);
            InitFight();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))GoBackPanel();
        }
        
        void InitFight()
        {
            allyInfoBox.InitBox(allyInstance);
            SetPokemonCapacities(allyInstance);
            enemyInfoBox.InitBox(enemyInstance);
        }

        void SetPokemonCapacities(PokemonInstance pokemon)
        {
            for (int i = 0; i < 4; i++)
            {
                if(pokemon.capacities[i] == null)continue;
                capacitiesButtons[i].SetCapacity(pokemon.capacities[i]);
            }
        }

        public void SetCapacity(int index)
        {
            var capacity = allyInstance.capacities[index];
            currentPPText.text = $"{capacity.currentPP}";
            maxPPText.text = $"{capacity.maxPP}";
            attackTypeText.text = $"{capacity.so.attackType}".ToUpper();
        }

        public void OpenFightPanel()
        {
            actionPanel.SetActive(false);
            fightPanel.SetActive(true);
            currentSelectedUI = Enums.UISelection.FightPanel;
            EventManager.Instance.eventSystem.SetSelectedGameObject(fightPanelFirstSelect.gameObject);
        }

        public void CloseFightPanel()
        {
            fightPanel.SetActive(false);
            actionPanel.SetActive(true);
            currentSelectedUI = Enums.UISelection.ActionMenu;
            EventManager.Instance.eventSystem.SetSelectedGameObject(actionPanelFirstSelect.gameObject);
        }

        public void GoBackPanel()
        {
            switch (currentSelectedUI)
            {
                case Enums.UISelection.ActionMenu:
                    break;
                case Enums.UISelection.FightPanel:
                    CloseFightPanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}