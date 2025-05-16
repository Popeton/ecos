using UnityEngine;
using System.Collections;
public class FadeInOutUI : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup del elemento UI
    public float fadeDuration = 1f; // Duración del efecto de FadeIn/FadeOut en segundos
    public float visibleDuration = 3f; // Tiempo que el elemento permanece visible antes de desaparecer

    private void Start()
    {
        if (canvasGroup == null) {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Configurar la transparencia inicial a 0 (invisible)
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
       
    }

    public void StartUIVisibility()
    {
        // Iniciar el FadeIn seguido del FadeOut
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        // FadeIn
        yield return StartCoroutine(FadeIn());

        // Esperar mientras el elemento es visible
        yield return new WaitForSeconds(visibleDuration);

        // FadeOut
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Asegurarse de que al final sea completamente visible
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Asegurarse de que al final sea completamente invisible
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}

