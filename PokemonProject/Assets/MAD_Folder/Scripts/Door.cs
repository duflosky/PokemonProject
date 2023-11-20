using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            // TODO : Animations, Music & Scene
            // SceneManager.LoadScene(sceneName);
        }
    }
}