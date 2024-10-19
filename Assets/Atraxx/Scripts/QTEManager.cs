using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private GameObject qteObject;
    [SerializeField] private TMP_Text qtePromptText;
    [SerializeField] private Image timerBar;
    [SerializeField] private float timeLimit = 3f;
    [SerializeField] private PlayerInput _playerInput;

    private bool _qteActive = false;
    private string _currentInput;
    private float timeRemaining;

    private void Update()
    {
        if (_qteActive)
        {
            timeRemaining -= Time.deltaTime;
            timerBar.fillAmount = timeRemaining / timeLimit;

            if (timeRemaining <= 0f)
            {
                QTEFailed();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartQTE();
        }
    }

    public void StartQTE()
    {
        qteObject.SetActive(true);
        _qteActive = true;
        timeRemaining = timeLimit;

        _currentInput = GetRandomInput();
        qtePromptText.text =_currentInput;

        timerBar.fillAmount = 1f;
        Debug.Log($"Esperando input: {_currentInput}");
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (_qteActive && context.phase == InputActionPhase.Performed)
        {
            string inputName = NormalizeInputName(context.control.displayName);
            Debug.Log($"Input recibido: {inputName}");

            if (inputName == _currentInput)
            {
                QTESuccess();
            }
            else
            {
                QTEFailed();
            }
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
            string[] keyboardInputs = { "V", "M", "N", "B" };
            return keyboardInputs[Random.Range(0, keyboardInputs.Length)];
        }
    }

    private bool IsGamepadActive()
    {
        foreach (var device in _playerInput.devices)
        {
            if (device is Gamepad) return true;
        }
        return false;
    }

    private string NormalizeInputName(string input)
    {
        // Normaliza el nombre del input para evitar problemas de comparación
        return input.Trim().ToUpper();
    }

    private void QTESuccess()
    {
        _qteActive = false;
        qtePromptText.text = "¡Éxito!";
        qteObject.SetActive(false);

    }

    private void QTEFailed()
    {
        _qteActive = false;
        qtePromptText.text = "¡Fallaste!";
        qteObject.SetActive(false);
    }
}
