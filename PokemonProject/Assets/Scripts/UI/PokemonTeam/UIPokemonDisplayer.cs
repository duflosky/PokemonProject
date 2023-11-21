using System;
using SO;
using TMPro;
using UI.Fight;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.PokemonTeam
{
    public class UIPokemonDisplayer : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private Image image;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI currentLifeText;
        [SerializeField] private TextMeshProUGUI maxLifeText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] Lifebar lifebar;
        [Header("Sprite")]
        [SerializeField]private Sprite selectSprite;
        private Sprite baseSprite;

        private void Start()
        {
            image = GetComponent<Image>();
            baseSprite = image.sprite;
        }

        public void InitDisplayer(PokemonInstance pokemon)
        {
            nameText.text = pokemon.name;
            currentLifeText.text = $"{pokemon.currentHp}";
            maxLifeText.text = $"{pokemon.maxHp}";
            levelText.text = $"{pokemon.level}";
            lifebar.InitBar(pokemon.currentHp, pokemon.maxHp);
        }

        public void OnSelect(BaseEventData eventData)
        {
            image.sprite = selectSprite;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            image.sprite = baseSprite;
        }
    }
}