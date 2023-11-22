using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Fight
{
    [Serializable]
    public struct barState
    {
        public Sprite fillSprite;
        public float lifePercent;
        
    }
    
    public class Lifebar : MonoBehaviour
    {
        public Image _image;
        
        [SerializeField] private barState[] barStates;

        private int maxLife;
        public int life;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void InitBar(int _life, int _maxLife)
        {
            life = _life;
            maxLife = _maxLife;
            UpdateBar();
        }

        public void UpdateBar()
        {
            float lifePercent = (float)life/maxLife; 
            _image.fillAmount = lifePercent;

            for (int i = 0; i < barStates.Length; i++)
            {
                if (lifePercent <= barStates[i].lifePercent)
                {
                    _image.sprite = barStates[i].fillSprite;
                    break;
                }
            }
        }
    }
}