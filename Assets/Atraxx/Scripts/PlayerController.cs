using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    public Rigidbody rb;

    [SerializeField] private float _speed = 5;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        if (direction!= Vector2.zero)
        {
            MovePlayer(direction);
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x,0, direction.y) * _speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
}
