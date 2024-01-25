using System.Collections;
using Manager;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapCollision;
    [SerializeField] private GameObject tilemapToActivate;
    [SerializeField] private GameObject tilemapToDeactivate;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isExit;

    private Fader fader;
    private CharacterController player;
    private CapsuleCollider2D collider;

    private void Start()
    {
        fader = UIManager.Instance.fader;
        player = GameManager.Instance.player;
        collider = player.GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(ChangeTilemap());
    }

    private IEnumerator ChangeTilemap()
    {
        if (animator != null && !isExit) animator.SetTrigger("Open");
        fader.FadeIn();
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        tilemapToDeactivate.SetActive(false);
        tilemapToActivate.SetActive(true);
        player.transform.position = position;
        player.GroundTilemap = tilemapFloor;
        player.CollisionTilemap = tilemapCollision;
        player.InitiateMovement(direction);
        fader.FadeOut();
        if (animator != null && isExit) animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
    }
}