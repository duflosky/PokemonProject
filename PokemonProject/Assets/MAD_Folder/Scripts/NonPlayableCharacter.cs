using System.Collections.Generic;
using UI;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    [SerializeField] private NonPlayableCharacterType type;
    [SerializeField] private List<string> dialogues;
    [SerializeField] private GameObject logPanel;
    [SerializeField] private UILogger uiLogger;
    [SerializeField] private CharacterController player;

    private int dialogueIndex;

    private void Update()
    {
        if (type != NonPlayableCharacterType.Walker) return;
        // TODO - NPC movement
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionAction += DisplayInteractionAction;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionAction -= DisplayInteractionAction;
    }

    private void DisplayInteractionAction()
    {
        if (dialogueIndex >= dialogues.Count)
        {
            logPanel.SetActive(false);
            dialogueIndex = 0;
            player.IsInteracting = false;
            return;
        }

        // TODO - According to the type of the NPC, fight can be triggered or not 
        logPanel.SetActive(true);
        player.IsInteracting = true;
        uiLogger.LogMessage(dialogues[dialogueIndex]);
        dialogueIndex++;
    }
}