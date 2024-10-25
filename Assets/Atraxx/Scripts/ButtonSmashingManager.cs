using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ButtonSmashingManager : MonoBehaviour
{
    [SerializeField] private GameObject sBObject; // Objeto del UI del desafío.
    [SerializeField] private TMP_Text promptText; // Texto del botón requerido.
    [SerializeField] private Image progressBar; // Barra de progreso.
    [SerializeField] private float timeLimit = 5f; // Tiempo total del desafío.
    [SerializeField] private float decayRate = 0.3f; // Velocidad a la que baja la barra.
    [SerializeField] private float fillAmountPerPress = 0.1f; // Incremento por input correcto.
    [SerializeField] private PlayerInput playerInput; // Componente de Input.

    private float timeRemaining;
    private bool challengeActive = false;
    private string currentInput; // Input requerido.

    private void Update()
    {
        if (challengeActive)
        {
            timeRemaining -= Time.deltaTime;

            progressBar.fillAmount = Mathf.Max(progressBar.fillAmount - decayRate * Time.deltaTime, 0f);

            if (timeRemaining <= 0f)
            {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartChallenge();
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
            //Debug.Log($"Input recibido: {inputName}");

            if (inputName == currentInput)
            {
                IncreaseProgress();
            }
        }
    }

    private void IncreaseProgress()
    {
        progressBar.fillAmount = Mathf.Min(progressBar.fillAmount + fillAmountPerPress, 1f);

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
        challengeActive = false;
        sBObject.SetActive(false); 
        promptText.text = "¡Éxito!"; 
        Debug.Log("¡Ganaste el desafío!");
    }

    private void ChallengeFailed()
    {
        challengeActive = false;
        sBObject.SetActive(false);
        promptText.text = "¡Fallaste!";
        Debug.Log("Se acabó el tiempo.");
    }
}
