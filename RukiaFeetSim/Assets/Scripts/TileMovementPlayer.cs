using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class TileMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float tileSize = 1f;        // Size of one tile in units
    public float moveDelay = 0.2f;     // Delay between moves
    public LayerMask obstacleLayer;    // Layer for walls/obstacles

    private float moveTimer = 0f;
    private Collider2D playerCollider;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();

        // Snap player to the tile grid on start
        Vector3 pos = transform.position;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);
    }

    private void Update()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer > 0f)
            return;

        Vector3 move = Vector3.zero;
        var keyboard = Keyboard.current;

        // Only allow one axis at a time (no diagonal movement)
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            move = Vector3.up;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            move = Vector3.down;
        else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            move = Vector3.left;
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            move = Vector3.right;

        if (move != Vector3.zero)
        {
            Vector3 targetPos = transform.position + move * tileSize;

            // Check collision using BoxCast for exact player collider size
            if (!Physics2D.BoxCast(targetPos, playerCollider.bounds.size, 0f, Vector2.zero, 0f, obstacleLayer))
            {
                transform.position = targetPos; // Move player
            }

            moveTimer = moveDelay; // Reset timer
        }
    }
}
