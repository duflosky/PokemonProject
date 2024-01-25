using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [Header("Graph")]
    [SerializeField] private GameObject graph;
    
    [Header("Controls")]
    [SerializeField] private float speed = 1;

    [Header("Tilemaps")]
    public Tilemap GroundTilemap;
    public Tilemap CollisionTilemap;
    public Tilemap GrassTilemap;
    [SerializeField] private GameObject grass;
    [SerializeField] private Animator animator;

    [Header("Physics")]
    [SerializeField] private CapsuleCollider2D collider;
    
    [Header("Jump")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpSpeed = 0.5f;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject dust;

    [HideInInspector] public bool IsInteracting;
    [HideInInspector] public bool IsWalking;
    [HideInInspector] public bool IsInFight;
    
    /* DELEGATES */
    public delegate void OnInteractionAction();
    public OnInteractionAction onInteractionAction;
    public delegate void OnInteractionMovement(Vector2 direction);
    public OnInteractionMovement onInteractionMovement;
    
    private PlayerInputs playerInputs;
    private Pool<GameObject> poolGrass;

    private void OnEnable()
    {
        playerInputs = new PlayerInputs();
        poolGrass = new Pool<GameObject>(grass, 10);
        playerInputs.InGame.Enable();
        playerInputs.InGame.Action.performed += _ => onInteractionAction?.Invoke();
    }

    private void OnDisable()
    {
        playerInputs.InGame.Disable();
        // TODO - event unsubscription via anonymous delegate
        // playerInputs.InGame.Action.performed -= _ => onInteraction?.Invoke(Vector2.zero);
    }

    private void Update()
    {
        if (IsWalking || IsInFight) return;
        var movement = playerInputs.InGame.Movement.ReadValue<Vector2>();
        onInteractionMovement?.Invoke(movement);
        if (!IsInteracting) InitiateMovement(movement);
    }

    public void InitiateMovement(Vector2 movement)
    {
        animator.SetBool("Walking", movement != Vector2.zero);
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

        animator.SetInteger("WalkSide", animIndex);
        animator.SetFloat("WalkBlend", animIndex / 3f);

        var targetPos = transform.position;
        targetPos += offset;

        StartCoroutine(Move(targetPos));
    }

    public void InitiateJump(Vector2 direction)
    {
        animator.SetBool("Walking", direction != Vector2.zero);
        if (direction == Vector2.zero) return;

        Vector3 offset;
        int animIndex;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                offset = Vector3.right * 2;
                animIndex = 1;
            }
            else
            {
                offset = Vector3.left * 2;
                animIndex = 3;
            }
        }
        else
        {
            if (direction.y > 0)
            {
                offset = Vector3.up * 2;
                animIndex = 0;
            }
            else
            {
                offset = Vector3.down * 2;
                animIndex = 2;
            }
        }

        animator.SetInteger("WalkSide", animIndex);
        animator.SetFloat("WalkBlend", animIndex / 3f);

        var targetPos = transform.position;
        targetPos += offset;

        shadow.SetActive(true);
        StartCoroutine(Jump());
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
                StartCoroutine(poolGrass.AddToPoolLater(grassGO,
                    grassGO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
            }
        }

        IsWalking = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        
        collider.enabled = true;
        transform.position = targetPos;
        IsWalking = false;
    }

    private IEnumerator Jump()
    {
        float expiredSeconds = 0.0f;
        float progress = 0.0f;
        Vector3 startPosition = graph.transform.localPosition;
        while (progress < 1.0f)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / jumpSpeed;
            graph.transform.localPosition = new Vector3(graph.transform.localPosition.x, startPosition.y + jumpCurve.Evaluate(progress) * 1.0f);   
            yield return null;
        }
        graph.transform.localPosition = startPosition;
        shadow.SetActive(false);
        dust.SetActive(true);
        StartCoroutine(DeactivateDust());
    }

    private IEnumerator DeactivateDust()
    {
        yield return new WaitForSeconds(dust.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        dust.SetActive(false);
    }
}