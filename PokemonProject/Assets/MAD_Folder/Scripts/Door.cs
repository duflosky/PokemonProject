using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapCollisions;
    [SerializeField] private GameObject tilemapToActivate;
    [SerializeField] private GameObject tilemapToDeactivate;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<CharacterController>();
            player.GroundTilemap = tilemapFloor;
            player.CollisionTilemaps = new[] {tilemapCollisions};
            tilemapToActivate.SetActive(true);
            tilemapToDeactivate.SetActive(false);
        }
    }
}