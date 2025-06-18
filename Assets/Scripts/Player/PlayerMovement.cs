using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField]
    private float moveSpeed = 4f;

    [Header("Réferences")]
    [SerializeField]
    private Rigidbody2D rb;

    public Animator anim;
    private Vector2 lastMoveDirection;

    private Vector2 moveDirection;
    private bool canMove = true;


    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            Vector3 spawnPos = GameManager.Instance.selectedCharacter.spawnPosition;
            transform.position = spawnPos;
        }

        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.SetPlayerInput(playerInput);
            }
            else
            {
                Debug.LogError("InputManager is not in the scene");
            }
        }
        else
        {
            Debug.LogError("Missing PlayerInput on GameObject");
        }
    }
    private void Update()
    {
        Animate();
        CheckMovementState();
    }

    private void CheckMovementState()
    {
        if (ObjectsManager.Instance != null)
        {
            bool textActive = ObjectsManager.Instance.IsTextActive();

            // Si l'état a changé
            if (canMove == textActive)
            {
                canMove = !textActive;

                // Si on bloque le mouvement, arrêter le joueur
                if (!canMove)
                {
                    moveDirection = Vector2.zero;
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    public void SetMoveDirection(Vector2 direction)
    {

        if (!canMove)
        {
            moveDirection = Vector2.zero;
            return;
        }
        moveDirection = direction;

        /*if (direction.x > 0 && isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && !isFacingRight)
        {
            Flip();
        } pour flip le sprite si symétrique plus simple que beaucoup d'animation */
    }


    /*private void Flip()
     {
       isFacingRight = !isFacingRight;
       Vector3 scale = transform.localScale;
       scale.x *= -1;
       transform.localScale = scale;
     }*/



    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        if (rb)
        {
            if (canMove)
            {
                rb.linearVelocity = moveDirection * moveSpeed;

            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            Debug.LogError("Rigidbody2D is missing on PlayerController");
        }
    }

    void Animate()
    {
        if (canMove && (moveDirection.x != 0 || moveDirection.y != 0))
        {
            lastMoveDirection = moveDirection;
        }
        Vector2 animMoveDirection = canMove ? moveDirection : Vector2.zero;

        anim.SetFloat("MoveX", moveDirection.x);
        anim.SetFloat("MoveY", moveDirection.y);
        anim.SetFloat("MoveMagnitude", moveDirection.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }
}
