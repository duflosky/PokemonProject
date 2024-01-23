using System.Collections.Generic;
using UI;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private List<string> dialogues;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject logPanel;
    [SerializeField] private UILogger uiLogger;
    
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
        if (dialogueIndex >= dialogues.Count)
        {
            logPanel.SetActive(false);
            dialogueIndex = 0;
            player.IsInteracting = false;
            return;
        }
        if (direction != this.direction) return;
        player.onInteractionMovement -= DisplayInteractionMovement;
        logPanel.SetActive(true);
        player.IsInteracting = true;
        // TODO - fix lock in walking animation
        uiLogger.LogMessage(dialogues[dialogueIndex]);
        dialogueIndex++;
    }
    
    private void DisplayInteractionAction()
    {
        DisplayInteractionMovement(direction);
    }
}