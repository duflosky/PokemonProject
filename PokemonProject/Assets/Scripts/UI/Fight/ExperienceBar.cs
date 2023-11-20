using UnityEngine;
using UnityEngine.UI;

namespace UI.Fight
{
    public class ExperienceBar : MonoBehaviour
    {
        private Image _image;
        
        int maxExp;
        int exp;
        private void Start()
        {
            _image = GetComponent<Image>();
        }
        
        public void InitBar(int _exp, int _maxExp)
        {
            maxExp = _maxExp;
            exp = _exp;
            UpdateBar();
        }

        void UpdateBar()
        {
            _image.fillAmount = (float)exp / maxExp;
        }
    }
}