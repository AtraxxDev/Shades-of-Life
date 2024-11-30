using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    public Rigidbody rb;
    public Transform playerCamera;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private bool canMove = true;

    [SerializeField]private Animator animator; // Referencia al componente Animator

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        playerInput.onControlsChanged += OnControlsChanged;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 direction = moveAction.ReadValue<Vector2>();

            // Actualiza el par�metro del Animator
            animator.SetBool("isWalking", direction != Vector2.zero);

            if (direction != Vector2.zero)
            {
                MovePlayer(direction);
            }
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * direction.y + camRight * direction.x;

        Vector3 move = moveDirection * _speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, newRotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == "Gamepad")
        {
            Debug.Log("Se est� usando un gamepad.");
        }
        else if (input.currentControlScheme == "Keyboard&Mouse")
        {
            Debug.Log("Se est� usando teclado y rat�n.");
        }
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
