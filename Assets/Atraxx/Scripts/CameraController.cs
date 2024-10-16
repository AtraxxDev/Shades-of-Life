using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera _camera;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
