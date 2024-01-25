using Manager;
using SO;
using UI;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    [SerializeField] private PokemonSO pokemon;

    private CharacterController player;
    private GameObject logPanel;
    private UILogger uiLogger;

    private void Start()
    {
        player = GameManager.Instance.player;
        logPanel = UIManager.Instance.logPanel;
        uiLogger = UIManager.Instance.uiLogger;
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
        if (GameManager.Instance.team[0] != null)
            if (GameManager.Instance.team[0]?.so == pokemon)
            {
                logPanel.SetActive(false);
                player.IsInteracting = false;
                player.onInteractionAction -= DisplayInteractionAction;
                return;
            }

        GameManager.Instance.team[0] = new PokemonInstance(pokemon, 5);
        logPanel.SetActive(true);
        player.IsInteracting = true;
        uiLogger.LogMessage("You received a " + pokemon.name + " !");
    }
}