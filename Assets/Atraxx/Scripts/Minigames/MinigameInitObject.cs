using UnityEngine;

public class MinigameInitObject : MonoBehaviour
{
    [SerializeField] private MinigameStarter minigameStarter;
    [SerializeField] private string NameSite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(NameSite))
        {
            Debug.Log("Toque el site");

            MinigameSite site = other.GetComponent<MinigameSite>();
            if (site != null)
            {
                site.TriggerMinigame(minigameStarter);
            }
        }
    }
}
