using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [SerializeField] private Image fadeImage;    // La imagen que hace el fade
    [SerializeField] private float fadeDuration = 1f; // Duración del fade en segundos

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Método general de Fade que puede hacer tanto FadeIn como FadeOut
    private IEnumerator PerformFade(float startAlpha, float endAlpha, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
        onComplete?.Invoke();
    }

    // Realiza un FadeIn
    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(PerformFade(0, 1, fadeDuration, onComplete));
    }

    // Realiza un FadeOut
    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(PerformFade(1, 0, fadeDuration, onComplete));
    }

    // Realiza un FadeIn seguido de un FadeOut
    public void FadeInOut(Action onComplete = null)
    {
        StartCoroutine(FadeInOutCoroutine(onComplete));
    }

    private IEnumerator FadeInOutCoroutine(Action onComplete)
    {
        // Realiza el FadeIn y luego espera un poco antes de hacer el FadeOut
        yield return PerformFade(0, 1, fadeDuration);  // FadeIn
        yield return new WaitForSeconds(0.5f);  // Espera antes de iniciar el FadeOut (ajustar tiempo según necesidad)
        yield return PerformFade(1, 0, fadeDuration);  // FadeOut

        onComplete?.Invoke();
    }
}
