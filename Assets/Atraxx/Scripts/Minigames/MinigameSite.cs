using UnityEngine;

public class MinigameSite : MonoBehaviour
{
    [SerializeField] private GameObject minigameObject;
    private IMinigame assignedMinigame;

    private void Start()
    {
        assignedMinigame = minigameObject.GetComponent<IMinigame>();

    }

    public void TriggerMinigame(MinigameStarter starter)
    {
        if(assignedMinigame != null)
        {
            starter.StartMinigame(assignedMinigame);
        }
    }
}
