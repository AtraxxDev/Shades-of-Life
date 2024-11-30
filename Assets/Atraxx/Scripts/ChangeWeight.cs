using UnityEngine;

public class ChangeWeight : MonoBehaviour
{
    public Animator animator; // Asigna tu Animator desde el inspector
    public int layerIndex = 1; // Índice del layer que quieres modificar

    // Cambia el peso del layer especificado a 1
    public void SetLayerWeightToOne()
    {
        if (animator == null || layerIndex >= animator.layerCount)
        {
            Debug.LogWarning("Animator no asignado o layerIndex inválido.");
            return;
        }

        animator.SetLayerWeight(layerIndex, 1.0f);
    }


    public void SetLayerWeightToZero()
    {
        if (animator == null || layerIndex >= animator.layerCount)
        {
            Debug.LogWarning("Animator no asignado o layerIndex inválido.");
            return;
        }

        animator.SetLayerWeight(layerIndex, 0.0f);
    }
}
