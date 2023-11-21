using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.PokemonTeam
{
    public class SelectActionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private Image image;
        [SerializeField] private Sprite selectedSprite;
        private Sprite baseSprite;

        private void Start()
        {
            image = GetComponent<Image>();
            baseSprite = image.sprite;
        }

        public void OnSelect(BaseEventData eventData)
        {
            image.sprite = selectedSprite;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            image.sprite = baseSprite;
        }
    }
}