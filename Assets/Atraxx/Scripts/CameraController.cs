using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
