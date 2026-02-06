using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TopDownMovement4Dir : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    public float castDistance = 0.05f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        float h =
            (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);

        float v =
            (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f);

        // Lock to 4 directions
        if (Mathf.Abs(h) > Mathf.Abs(v))
            movementInput = new Vector2(Mathf.Sign(h), 0f);
        else if (Mathf.Abs(v) > 0f)
            movementInput = new Vector2(0f, Mathf.Sign(v));
        else
            movementInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        Vector2 desiredVelocity = movementInput * moveSpeed;

        if (movementInput != Vector2.zero)
        {
            // Check if movement direction is blocked
            RaycastHit2D hit = Physics2D.BoxCast(
                rb.position,
                col.bounds.size,
                0f,
                movementInput,
                castDistance,
                obstacleLayer
            );

            if (hit)
            {
                desiredVelocity = Vector2.zero;
            }
        }

        rb.linearVelocity = desiredVelocity;
    }
}
