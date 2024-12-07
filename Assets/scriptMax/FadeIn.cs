using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa si usas TextMeshPro

public class FadeInWithTrigger : MonoBehaviour
{
    public Image fadeImage; // Imagen negra del Canvas
    public TextMeshProUGUI fadeText; // Texto que aparecerá (usa TextMeshPro)
    public float fadeDuration = 1.0f; // Tiempo para oscurecer la pantalla
    public float textFadeDuration = 1.0f; // Tiempo para que el texto aparezca
    public string triggerTag = "Player"; // Etiqueta para detectar al jugador

    private bool isFading = false;
    private bool isTextFading = false;
    private float fadeTimer = 0;
    private float textFadeTimer = 0;

    void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0; // La pantalla comienza transparente
            fadeImage.color = color;
        }

        if (fadeText != null)
        {
            Color textColor = fadeText.color;
            textColor.a = 0; // El texto comienza invisible
            fadeText.color = textColor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detecta si el objeto que entra tiene la etiqueta especificada
        if (other.CompareTag(triggerTag))
        {
            StartFadeIn(); // Inicia el efecto de fade in
        }
    }

    void StartFadeIn()
    {
        if (!isFading)
        {
            isFading = true;
            fadeTimer = 0;
        }
    }

    void Update()
    {
        // Manejar el fade in de la pantalla
        if (isFading && fadeImage != null)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            Color color = fadeImage.color;
            color.a = alpha; // Incrementa la transparencia hacia 1
            fadeImage.color = color;

            if (alpha >= 1) // Cuando llega a opaco, comienza el fade in del texto
            {
                isFading = false;
                isTextFading = true;
                textFadeTimer = 0; // Reinicia el temporizador para el texto
            }
        }

        // Manejar el fade in del texto
        if (isTextFading && fadeText != null)
        {
            textFadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(textFadeTimer / textFadeDuration);
            Color textColor = fadeText.color;
            textColor.a = alpha; // Incrementa la transparencia del texto hacia 1
            fadeText.color = textColor;

            if (alpha >= 1) // Cuando el texto está completamente visible, detiene el fade
            {
                isTextFading = false;
            }
        }
    }
}

