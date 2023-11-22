using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [Header("Controls")] [SerializeField] private float speed = 1;

    [Header("Tilemaps")] public Tilemap GroundTilemap;
    public Tilemap CollisionTilemap;
    public Tilemap GrassTilemap;
    [SerializeField] private GameObject grass;
    [SerializeField] private Animator _animator;

    [Header("Physics")] [SerializeField] private CapsuleCollider2D collider;

    private bool isWalking;
    private PlayerInputs playerInputs;
    private Pool<GameObject> poolGrass;

    private void OnEnable()
    {
        playerInputs = new PlayerInputs();
        poolGrass = new Pool<GameObject>(grass, 10);
        playerInputs.InGame.Enable();
    }

    private void OnDisable()
    {
        playerInputs.InGame.Disable();
    }

    private void Update()
    {
        if (isWalking) return;
        var movement = playerInputs.InGame.Movement.ReadValue<Vector2>();
        InitiateMovement(movement);
    }

    public void InitiateMovement(Vector2 movement)
    {
        _animator.SetBool("Walking", movement != Vector2.zero);
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
        _animator.SetFloat("WalkBlend", animIndex / 3f);

        var targetPos = transform.position;
        targetPos += offset;

        StartCoroutine(Move(targetPos));
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        Vector3Int gridPosition = GroundTilemap.WorldToCell(targetPos);
        if (!GroundTilemap.HasTile(gridPosition) || CollisionTilemap.HasTile(gridPosition))
        {
            yield break;
        }

        if (GrassTilemap != null)
        {
            if (GrassTilemap.HasTile(gridPosition))
            {
                var grassGO = poolGrass.GetFromPool();
                grassGO.transform.position = targetPos;
                StartCoroutine(poolGrass.AddToPoolLater(grassGO, grassGO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
            }
        }

        isWalking = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        collider.enabled = true;
        transform.position = targetPos;
        isWalking = false;
    }
}