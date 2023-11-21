using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [Header("Controls")] [SerializeField] private int speed = 1;

    [Header("Tilemaps")] [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap[] collisionTilemaps;

    private bool isWalking;
    private PlayerInputs playerInputs;
    private Vector2 movement;

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

        if (movement.x != 0)
        {
            movement.x = Mathf.RoundToInt(movement.x);
            movement.y = 0;
        }

        if (movement.y != 0)
        {
            movement.y = Mathf.RoundToInt(movement.y);
            movement.x = 0;
        }

        if (movement == Vector2.zero) return;
        var targetPos = transform.position;
        targetPos.x += movement.x;
        targetPos.y += movement.y;

        StartCoroutine(Move(targetPos));
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(targetPos);
        foreach (var collisionTilemap in collisionTilemaps)
        {
            if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
            {
                yield break;
            }
        }

        isWalking = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isWalking = false;
    }
}