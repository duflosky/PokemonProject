using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public void FadeIn()
    {
        image.DOFade(1, 0.5f);
    }
    
    public void FadeOut()
    {
        image.DOFade(0, 0.5f);
    }
}
