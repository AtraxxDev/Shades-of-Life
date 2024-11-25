using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class SkillCheckManager : MonoBehaviour, IMinigame
{
    public bool IsComplete { get; private set; } = false;

    // UI Elements
    [Header("UI Elements")]
    [SerializeField] private GameObject skillCheckUI;
    [SerializeField] private Image movingBar;
    [SerializeField] private Image targetArea;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator Animation;
    [Space(10)]
    // Configuration
    [Header("Configuration")]
    [SerializeField] private float barSpeed = 100f;
    private const float failMargin = 20f;

    [Space(10)]
    // State Variables
    [Header("State Variables")]
    private RectTransform barTransform;
    private RectTransform targetTransform;
    private RectTransform backgroundTransform;
    private Vector2 initialBarPosition;
    private bool skillCheckActive = false;

    // Reference to MinigameStarter
    [SerializeField]private MinigameStarter minigameStarter;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        
        if (skillCheckActive)
        {
            MoveBar();
            CheckSkillCheckFail();
        }
    }

    private void Initialize()
    {
        barTransform = movingBar.GetComponent<RectTransform>();
        targetTransform = targetArea.GetComponent<RectTransform>();
        backgroundTransform = background.GetComponent<RectTransform>();
        skillCheckUI.SetActive(false);

        // Initial bar position
        initialBarPosition = new Vector2(backgroundTransform.anchoredPosition.x - (backgroundTransform.sizeDelta.x / 2), -157);
        barTransform.anchoredPosition = initialBarPosition;
    }

    private void MoveBar()
    {
        barTransform.anchoredPosition += new Vector2(barSpeed * Time.deltaTime, 0);
    }

    private void CheckSkillCheckFail()
    {
        if (barTransform.anchoredPosition.x > (targetTransform.anchoredPosition.x + (targetArea.rectTransform.sizeDelta.x / 2) + failMargin))
        {
            SkillCheckFailed();
        }
    }

    public void StartSkillCheck()
    {
        skillCheckActive = true;
        skillCheckUI.SetActive(true);
        promptText.text = IsGamepadActive() ? "A" : "V";

        ResetBarPosition();
        SetRandomTargetPosition();
    }

    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-140f, 200f);
        targetTransform.anchoredPosition = new Vector2(randomX, targetTransform.anchoredPosition.y);
    }

    public void OnSkillCheckInput(InputAction.CallbackContext context)
    {
        if (skillCheckActive && context.phase == InputActionPhase.Performed)
        {
            string inputName = NormalizeInputName(context.control.displayName);
            Debug.Log($"Input recibido: {inputName}");

            if (IsValidInput(inputName) && IsWithinTargetArea())
            {
                SkillCheckSuccess();
            }
            else
            {
                SkillCheckFailed();
            }
        }
    }

    private bool IsWithinTargetArea()
    {
        float barPos = barTransform.anchoredPosition.x;
        float targetPos = targetTransform.anchoredPosition.x;
        float targetWidth = targetArea.rectTransform.sizeDelta.x / 2;

        return barPos >= (targetPos - targetWidth) && barPos <= (targetPos + targetWidth);
    }

    private bool IsValidInput(string inputName)
    {
        return (IsGamepadActive() && inputName == "A") || (!IsGamepadActive() && inputName == "V");
    }

    private bool IsGamepadActive()
    {
        return playerInput.devices.Any(device => device is Gamepad);
    }

    private string NormalizeInputName(string input)
    {
        return input.Trim().ToUpper();
    }

    private void SkillCheckSuccess()
    {
        skillCheckActive = false;
        skillCheckUI.SetActive(false);
        Debug.Log("¡Skill Check exitoso!");
        promptText.text = "¡Éxito!";
        ResetBarPosition();
        IsComplete = true;

        // Notify MinigameStarter
        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }
    }

    private void SkillCheckFailed()
    {
        skillCheckActive = false;
        skillCheckUI.SetActive(false);
        Debug.Log("Skill Check fallido.");
        promptText.text = "¡Fallaste!";
        ResetBarPosition();

        // Notify MinigameStarter
        if (minigameStarter != null)
        {
            minigameStarter.EndCurrentMinigame(this);
        }
    }

    private void ResetBarPosition()
    {
        barTransform.anchoredPosition = initialBarPosition;
    }

    // IMinigame Interface Implementation
    public void StartMinigame()
    {

        Debug.Log("Iniciando minijuego de Skill Check...");
        FadeManager.Instance.FadeInOut(() =>
        {
            // Lógica después del fade-in
            StartSkillCheck();
        });
        


    }

    public void EndMinigame()
    {
        Debug.Log("Terminando minijuego de Skill Check...");

        FadeManager.Instance.FadeInOut(() =>
        {
            if (Animation != null)
            {
                Animation.SetTrigger("End");
            }
            skillCheckActive = false;
            skillCheckUI.SetActive(false);
        });

    }
}
