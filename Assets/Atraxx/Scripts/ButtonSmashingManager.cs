using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ButtonSmashingManager : MonoBehaviour, IMinigame
{
    public bool IsComplete { get; private set; } = false;


    [SerializeField] private GameObject sBObject; // Objeto del UI del desafío.
    [SerializeField] private TMP_Text promptText; // Texto del botón requerido.
    [SerializeField] private Image progressBar; // Barra de progreso.
    [SerializeField] private float timeLimit = 5f; // Tiempo total del desafío.
    [SerializeField] private float decayRate = 0.3f; // Velocidad a la que baja la barra.
    [SerializeField] private float fillAmountPerPress = 0.1f; // Incremento por input correcto.
    [SerializeField] private PlayerInput playerInput; // Componente de Input.
    [SerializeField] private MinigameStarter minigameStarter; // Referencia para terminar el minijuego.
    [SerializeField] private Animator Animation;
    public string tagToFind;

    private float timeRemaining;
    private bool challengeActive = false;
    private string currentInput; // Input requerido.

    public void StartMinigame()
    {
        FadeManager.Instance.FadeInOut(() =>
        {
            StartChallenge();
        });
    }

    public void EndMinigame()
    {
        FadeManager.Instance.FadeInOut(() =>
        {
            // Aquí puedes agregar cualquier acción después de que el fade termine
        });
    }

    private void Update()
    {
        if (challengeActive)
        {
            timeRemaining -= Time.deltaTime;
            progressBar.fillAmount = Mathf.Max(progressBar.fillAmount - decayRate * Time.deltaTime, 0f);

            if (timeRemaining <= 0f)
            {
                // Si el tiempo se acaba, evaluamos si ganó o perdió
                if (Mathf.Approximately(progressBar.fillAmount, 1f))
                {
                    ChallengeSuccess();
                }
                else
                {
                    ChallengeFailed();
                }
            }
        }
    }

    private void StartChallenge()
    {
        challengeActive = true;
        timeRemaining = timeLimit;
        progressBar.fillAmount = 0f;

        currentInput = GetRandomInput();
        promptText.text = currentInput;
        sBObject.SetActive(true);
    }

    public void OnSmashAction(InputAction.CallbackContext context)
    {
        if (challengeActive && context.phase == InputActionPhase.Performed)
        {
            string inputName = NormalizeInputName(context.control.displayName);

            if (inputName == currentInput)
            {
                IncreaseProgress();
            }
        }
    }

    private void IncreaseProgress()
    {
        progressBar.fillAmount = Mathf.Min(progressBar.fillAmount + fillAmountPerPress, 1f);

        // Si la barra llega al máximo, significa que el jugador completó el desafío con éxito
        if (Mathf.Approximately(progressBar.fillAmount, 1f))
        {
            ChallengeSuccess();
        }
    }

    private string GetRandomInput()
    {
        if (IsGamepadActive())
        {
            string[] gamepadInputs = { "A", "B", "X", "Y" };
            return gamepadInputs[Random.Range(0, gamepadInputs.Length)];
        }
        else
        {
            string[] keyboardInputs = { "V", "B", "N", "M" };
            return keyboardInputs[Random.Range(0, keyboardInputs.Length)];
        }
    }

    private bool IsGamepadActive()
    {
        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad) return true;
        }
        return false;
    }

    private string NormalizeInputName(string input)
    {
        return input.Trim().ToUpper();
    }

    private void ChallengeSuccess()
    {
        // Desactivar el desafío de inmediato
        challengeActive = false;
        sBObject.SetActive(false);  // Desactiva el UI
        promptText.text = "¡Éxito!"; // Muestra el mensaje de éxito
        Debug.Log("¡Ganaste el desafío!");

        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }

        FadeManager.Instance.FadeInOut(() =>
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tagToFind);
            foreach (GameObject obj in objects)
            {
                Animator anim = obj.GetComponent<Animator>();
                {

                    if (anim != null)
                    {
                        anim.SetTrigger("PastoUno");
                    }
                }
            }
            if (Animation != null)
            {
                Animation.SetTrigger("EndII");
            }
        });
    }

    private void ChallengeFailed()
    {
        challengeActive = false;
        sBObject.SetActive(false); // Desactiva el UI
        promptText.text = "¡Fallaste!"; // Muestra el mensaje de fracaso
        Debug.Log("Se acabó el tiempo.");
        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }
        FadeManager.Instance.FadeInOut(() =>
        {
            // Aquí podrías agregar más acciones luego del fade, si es necesario
        });
    }
}
