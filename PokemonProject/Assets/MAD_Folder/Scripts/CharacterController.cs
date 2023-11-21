using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [Header("Controls")] [SerializeField] private int speed = 1;

    [Header("Tilemaps")] [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap[] collisionTilemaps;
    [SerializeField] private Animator _animator;

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
        _animator.SetBool("Walking", isWalking);
        if (isWalking) return;
        movement = playerInputs.InGame.Movement.ReadValue<Vector2>();
        if (movement == Vector2.zero) return;

        Vector3 offset;
        int animIndex;
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            if (movement.x > 0)
            {
                offset = Vector3.right;
                animIndex = 1;
            }
            else
            {
                offset = Vector3.left;
                animIndex = 3;
            }
        }
        else
        {
            if (movement.y > 0)
            {
                offset = Vector3.up;
                animIndex = 0;
            }
            else
            {
                offset = Vector3.down;
                animIndex = 2;
            }
        }
        
        _animator.SetInteger("WalkSide", animIndex);
        _animator.SetFloat("WalkBlend", animIndex/3f);

        var targetPos = transform.position;
        targetPos += offset;

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