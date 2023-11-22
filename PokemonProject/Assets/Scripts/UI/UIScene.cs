using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIScene : MonoBehaviour
    {
        public static UIScene Instance;
        [SerializeField] private Image easeImage;
        [SerializeField] private float easeTime = 1;

        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            easeImage.gameObject.SetActive(false);
        }

        public async Task EaseIn(float duration)
        {
            var col = new Color(easeImage.color.r,easeImage.color.g,easeImage.color.b,0);
            easeImage.color = col;
            easeImage.gameObject.SetActive(true);
            float timer = 0;
            while (col.a<1)
            {
                timer += Time.deltaTime;
                col.a= timer/duration;
                easeImage.color = col;
                await Task.Yield();
            }
        }
        
        public async Task EaseOut(float duration)
        {
            var col = new Color(easeImage.color.r,easeImage.color.g,easeImage.color.b,1);
            easeImage.color = col;
            easeImage.gameObject.SetActive(true);
            float timer = 0;
            while (col.a>0)
            {
                timer += Time.deltaTime;
                col.a= 1- timer/duration;
                easeImage.color = col;
                await Task.Yield();
            }
            easeImage.gameObject.SetActive(false);
        }
    }
}