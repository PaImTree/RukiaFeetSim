using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TileMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float tileSize = 1f;
    public float moveDelay = 0.2f;
    public float moveSpeed = 10f;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 targetPosition;
    private float moveTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        // Snap to grid
        Vector2 startPos = rb.position;
        startPos.x = Mathf.Round(startPos.x);
        startPos.y = Mathf.Round(startPos.y);
        rb.position = startPos;

        targetPosition = rb.position;
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;

        // Still moving or waiting
        if (moveTimer > 0f || Vector2.Distance(rb.position, targetPosition) > 0.01f)
            return;

        Vector2 inputDir = Vector2.zero;
        var keyboard = Keyboard.current;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            inputDir = Vector2.up;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            inputDir = Vector2.down;
        else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            inputDir = Vector2.left;
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            inputDir = Vector2.right;

        if (inputDir != Vector2.zero)
        {
            Vector2 proposedTarget = rb.position + inputDir * tileSize;

            // Cast FROM current position TOWARD target
            RaycastHit2D hit = Physics2D.BoxCast(
                rb.position,
                col.bounds.size,
                0f,
                inputDir,
                tileSize,
                obstacleLayer
            );

            if (!hit)
            {
                targetPosition = proposedTarget;
                moveTimer = moveDelay;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            )
        );
    }
}