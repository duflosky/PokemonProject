using UnityEngine;

public class OutPalettScript : MonoBehaviour
{
    private CharacterController player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<CharacterController>();
            // TODO - check if player already has the pokemon
            Debug.Log("Out of Palett Town!");
        }
    }
}
