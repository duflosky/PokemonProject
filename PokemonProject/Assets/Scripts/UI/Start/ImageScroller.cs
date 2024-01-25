using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageScroller : MonoBehaviour
{

    [SerializeField] private GameObject[] images;
    [Space] 
    [SerializeField] private ImageScroller nextScroller;

    private int index = 0;
    private bool isStart;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var image in images)
        {
            image.SetActive(false);
        }
    }

    public void StartSequence()
    {
        index = 0;
        images[0].SetActive(true);
    }

    public ImageScroller GoNext()
    {
        if (index == images.Length -1)
        {
            if(nextScroller == null)return null;
            nextScroller.StartSequence();
            images[index].SetActive(false);
            return nextScroller;
        }
        images[index].SetActive(false);
        images[++index].SetActive(true);
        return this;
    }

    public void GoPrevious()
    {
        if(index == 0 )return;
        images[index].SetActive(false);
        images[--index].SetActive(true);
    }
}
