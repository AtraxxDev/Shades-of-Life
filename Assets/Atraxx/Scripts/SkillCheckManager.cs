using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SkillCheckManager : MonoBehaviour
{
    // UI Elements
    [Header("UI Elements")]
    [SerializeField] private GameObject skillCheckUI;
    [SerializeField] private Image movingBar;
    [SerializeField] private Image targetArea;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private PlayerInput playerInput;

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

        // Start skill check manually for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSkillCheck();
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
        // Check if the bar has exceeded the target area (including margin)
        if (barTransform.anchoredPosition.x > (targetTransform.anchoredPosition.x + (targetArea.rectTransform.sizeDelta.x / 2) + failMargin))
        {
            SkillCheckFailed();
        }
    }

    private void StartSkillCheck()
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

            // Check for valid input and target area
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

    private void SkillCheckSuccess()
    {
        skillCheckActive = false;
        skillCheckUI.SetActive(false);
        Debug.Log("¡Skill Check exitoso!");
        promptText.text = "¡Éxito!";
        ResetBarPosition();
    }

    private void SkillCheckFailed()
    {
        skillCheckActive = false;
        skillCheckUI.SetActive(false);
        Debug.Log("Skill Check fallido.");
        promptText.text = "¡Fallaste!";
        ResetBarPosition();
    }

    private void ResetBarPosition()
    {
        barTransform.anchoredPosition = initialBarPosition;
    }
}
