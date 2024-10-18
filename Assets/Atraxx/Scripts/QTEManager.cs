using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _qtepromptText;
    [SerializeField] private Image timerBar;
    [SerializeField] private float timeLimit = 3f;

    private PlayerInput _playerInput;
    private bool _qteActive = false;
    private string _currentInput;
    private float _timeRemaining;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_qteActive)
        {
            _timeRemaining -= Time.deltaTime;
            timerBar.fillAmount = _timeRemaining / timeLimit;

            if (_timeRemaining <= 0f)
            {
                QTEFailed();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartQTE();
        }
    }

    public void StartQTE()
    {
        _qteActive = true;
        _timeRemaining = timeLimit;

        // Elegimos un input adecuado según el control activo (teclado o gamepad)
        _currentInput = GetRandomInput();
        _qtepromptText.text = $"Presiona: {_currentInput}";

        timerBar.fillAmount = 1f; // Reiniciar la barra de tiempo
    }

    private void QTESuccess()
    {
        _qteActive = false;
        _qtepromptText.text = "¡Éxito!";
        // Lógica adicional al completar con éxito el QTE
    }

    private void QTEFailed()
    {
        _qteActive = false;
        _qtepromptText.text = "¡Fallaste!";
        // Lógica adicional para manejar el fallo
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (_qteActive && context.phase == InputActionPhase.Performed)
        {
            string inputName = context.control.displayName.ToUpper(); // Detectamos el nombre del input

            if (inputName == _currentInput)  // Comparar con la entrada requerida
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
        string controlScheme = _playerInput.currentControlScheme;

        if (controlScheme == "Gamepad")
        {
            // Botones típicos de gamepad
            string[] gamepadInputs = { "Cross", "Circle", "Square", "Triangle" };
            return gamepadInputs[Random.Range(0, gamepadInputs.Length)];
        }
        else // Keyboard&Mouse
        {
            // Teclas comunes de teclado
            string[] keyboardInputs = { "A", "S", "D", "F" };
            return keyboardInputs[Random.Range(0, keyboardInputs.Length)];
        }
    }
}
