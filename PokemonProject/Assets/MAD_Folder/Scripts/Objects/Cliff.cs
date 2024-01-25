using UnityEngine;

public class Cliff : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private CharacterController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionMovement += JumpOverCliff;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionMovement -= JumpOverCliff;
    }

    private void JumpOverCliff(Vector2 direction)
    {
        if (direction != this.direction) return;
        player.onInteractionMovement -= JumpOverCliff;
        player.InitiateJump(direction);
    }
}