using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class QTEManager : MonoBehaviour, IMinigame
{
    public bool IsComplete { get; private set; } = false;


    [SerializeField] private GameObject qteObject;
    [SerializeField] private TMP_Text qtePromptText;
    [SerializeField] private Image timerBar;
    [SerializeField] private float timeLimit = 3f;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator Animation;
    public string tagToFind;

    private bool _qteActive = false;
    private string _currentInput;
    private float timeRemaining;

    [SerializeField] private MinigameStarter minigameStarter; // Referencia para terminar el minijuego.

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


    }


    public void StartMinigame()
    {
        FadeManager.Instance.FadeInOut(() =>
        {
            StartQTE();
        });
    }

    public void EndMinigame()
    {
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
                Animation.SetTrigger("EndIII");
            }
        });
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
        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }

    }

    private void QTEFailed()
    {
        _qteActive = false;
        qtePromptText.text = "¡Fallaste!";
        qteObject.SetActive(false);
        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }
    }

    
}
