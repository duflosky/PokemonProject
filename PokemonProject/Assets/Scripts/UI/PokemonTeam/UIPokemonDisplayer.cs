using System;
using System.Collections;
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
        [SerializeField] private Image animationImage;
        [Header("Sprite")]
        [SerializeField]private Sprite selectSprite;

        [Space] [SerializeField] private float animationDelay= 1;
        
        private Sprite baseSprite;

        private Sprite[] pokemonAnimationFrames;


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
            pokemonAnimationFrames = pokemon.so.animationFrames;
            StartCoroutine(AnimateSprite());
        }

        public void OnSelect(BaseEventData eventData)
        {
            image.sprite = selectSprite;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            image.sprite = baseSprite;
        }

        IEnumerator AnimateSprite()
        {
            int frameCount = 0;
            while (true)
            {
                if (pokemonAnimationFrames == null) break;
                
                frameCount++;
                if (frameCount == pokemonAnimationFrames.Length) frameCount = 0;

                animationImage.sprite = pokemonAnimationFrames[frameCount];
                
                yield return new WaitForSeconds(animationDelay);
            }
        }
    }
}