using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float horizontalSpeed = 300f;  // Velocidad horizontal
    [SerializeField] private float verticalSpeed = 100f;    // Velocidad vertical

    private CinemachineOrbitalFollow orbitalFollow;
    private Vector2 lookInput;

    private void Awake()
    {
        orbitalFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (orbitalFollow != null)
        {
            // Ajustar el eje horizontal basado en la entrada del jugador
            float newHorizontalValue = orbitalFollow.HorizontalAxis.Value +
                                       lookInput.x * horizontalSpeed * Time.deltaTime;

            // Ajustar el eje vertical basado en la entrada del jugador
            float newVerticalValue = orbitalFollow.VerticalAxis.Value -
                                     lookInput.y * verticalSpeed * Time.deltaTime;
        }
    }
}
