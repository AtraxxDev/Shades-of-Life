using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float mouseSensitivity = 1.0f;

    private PlayerInputs playerInput;
    private Vector2 lookInput;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake()
    {
        playerInput = new PlayerInputs();

        playerInput.PlayerActions.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.PlayerActions.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => playerInput.PlayerActions.Enable();
    private void OnDisable() => playerInput.PlayerActions.Disable();
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -30f, 60f); 

        rotationY += mouseX;

        virtualCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
