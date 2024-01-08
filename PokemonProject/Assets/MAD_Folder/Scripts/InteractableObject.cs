using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private List<string> dialogues;
    [SerializeField] private Vector2 direction;
    
    private CharacterController player;
    private int dialogueIndex;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<CharacterController>();
            player.onInteractionMovement += DisplayInteractionMovement;
            player.onInteractionAction += DisplayInteractionAction;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.onInteractionMovement -= DisplayInteractionMovement;
            player.onInteractionAction -= DisplayInteractionAction;
        }
    }
    
    private void DisplayInteractionMovement(Vector2 direction)
    {
        if (direction != this.direction) return;
        player.onInteractionMovement -= DisplayInteractionMovement;
        Debug.Log("I can't move!");
        player.IsInteracting = true;
        // TODO - fix lock in walking animation
        // TODO - Display dialogue
        Debug.Log($"Displaying dialogue: {dialogues[dialogueIndex]}");
        dialogueIndex++;
        if (dialogueIndex >= dialogues.Count)
        {
            Debug.Log("I can move again!");
            dialogueIndex = 0;
            player.IsInteracting = false;
        }
    }
    
    private void DisplayInteractionAction()
    {
        DisplayInteractionMovement(direction);
    }
}