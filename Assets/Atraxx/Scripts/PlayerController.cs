using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    public Rigidbody rb;
    public Transform playerCamera; // Referencia al transform de la c�mara
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _rotationSpeed = 10f; // Velocidad de rotaci�n

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        if (direction != Vector2.zero)
        {
            MovePlayer(direction);
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        // Obtener la direcci�n de movimiento basada en la direcci�n de la c�mara
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        // Eliminar la componente Y para que el movimiento sea horizontal
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calcular la direcci�n del movimiento en el espacio del mundo
        Vector3 moveDirection = camForward * direction.y + camRight * direction.x;

        // Mover al jugador
        Vector3 move = moveDirection * _speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotar al jugador hacia la direcci�n del movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, newRotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
