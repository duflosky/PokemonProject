using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [Header("Controls")] [SerializeField] private int speed = 5;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    private bool isWalking;
    private Vector2 movement;
    private PlayerInputs playerInputs;

    private void OnEnable()
    {
        playerInputs = new PlayerInputs();
        playerInputs.InGame.Enable();
    }

    private void OnDisable()
    {
        playerInputs.InGame.Disable();
    }

    private void Update()
    {
        if (isWalking) return;
        movement = playerInputs.InGame.Movement.ReadValue<Vector2>();

        if (movement.x != 0) movement.y = 0;

        if (movement == Vector2.zero) return;
        var targetPos = transform.position;
        targetPos.x += movement.x;
        targetPos.y += movement.y;

        StartCoroutine(Move(targetPos));
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isWalking = true;

        Vector3Int gridPosition = groundTilemap.WorldToCell(targetPos);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            isWalking = false;
            yield break;
        }

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isWalking = false;
    }
}