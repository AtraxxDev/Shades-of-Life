using UnityEngine;

public abstract class Interactable : MonoBehaviour, IPickable
{
    [SerializeField] private Collider triggerCollider;

    private bool isPickedUp = false;
    public virtual void PickUp(Transform holder)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            transform.SetParent(holder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<Rigidbody>().isKinematic = true;
            triggerCollider.enabled = false;
        }
    }
    public virtual void Drop()
    {
        if (isPickedUp)
        {
            isPickedUp = false;
            transform.SetParent(null);
            GetComponent<Rigidbody>().isKinematic = false;
            triggerCollider.enabled = true;
        }
    }

    public abstract void Interact();
}
