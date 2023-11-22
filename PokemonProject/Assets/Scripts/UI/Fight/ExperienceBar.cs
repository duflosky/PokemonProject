using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Fight
{
    public class ExperienceBar : MonoBehaviour
    {

        [SerializeField] private float tickSize = 0.01f;
        
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

        public async Task UpdateExperience(int _exp)
        {
            exp = _exp;
            var progression = _image.fillAmount;
            while (progression +  tickSize < (float)exp / maxExp)
            {
                progression += tickSize * Time.deltaTime;
                _image.fillAmount = progression;
                await Task.Yield();
            }
        }
    }
}