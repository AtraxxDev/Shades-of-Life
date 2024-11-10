using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Transform rayOrigin;
    public Transform itemHolder;
    public float rayDistance = 1.2f;
    public LayerMask pickableLayer;

    private IPickable pickedObject;
    private PlayerInputs playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.PlayerActions.Interact.performed += OnInteract;
    }

    void OnEnable()
    {
        playerInputs.PlayerActions.Enable();
    }

    void OnDisable()
    {
        playerInputs.PlayerActions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (pickedObject != null)
        {
            DropObject();
        }
        else
        {
            TryPickUpObject();
        }
    }

    void TryPickUpObject()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, pickableLayer))
        {
            IPickable pickable = hit.collider.GetComponent<IPickable>();
            if (pickable != null)
            {
                pickable.PickUp(itemHolder);
                pickedObject = pickable;
            }
        }
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
            pickedObject.Drop();
            pickedObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (rayOrigin == null) return; 

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, pickableLayer))
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Vector3 rayStart = rayOrigin.position;
        Vector3 rayEnd = rayOrigin.position + rayOrigin.forward * rayDistance;

        Gizmos.DrawLine(rayStart, rayEnd);

    }
}
