using UnityEngine;

public class MinigameStarter : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private IMinigame _currentMinigame;

    public void StartMinigame(IMinigame miniGame)
    {
        // Verifica si el minijuego ya está completo
        if (miniGame is IMinigame completableMinigame && completableMinigame.IsComplete)
        {
            Debug.Log("Este minijuego ya fue completado. No se iniciará de nuevo.");
            return;
        }

        // Asigna el minijuego y deshabilita el movimiento
        _currentMinigame = miniGame;
        _currentMinigame.StartMinigame();
        playerController.SetCanMove(false);
    }

    public void EndCurrentMinigame(IMinigame miniGame)
    {
        if (_currentMinigame != null)
        {
            _currentMinigame.EndMinigame();
            _currentMinigame = null;
            playerController.SetCanMove(true);
        }
    }
}
