using UnityEngine;

public interface IPickable
{
    void PickUp(Transform holder);
    void Drop();
}
