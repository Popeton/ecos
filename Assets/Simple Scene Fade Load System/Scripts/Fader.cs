using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    private Image fadeImage;
    private Color fadeColor;
    private float fadeDuration;
    private string targetScene;

    private bool isFadingOut = true;

    private CanvasGroup canvasGroup;

    public void Setup(Image image, Color color, float duration, string scene)
    {
        fadeImage = image;
        fadeColor = color;
        fadeDuration = duration;
        targetScene = scene;

        // Configurar el canvas group si no existe
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeOutIn());
    }

    private IEnumerator FadeOutIn()
    {
        float timer = 0f;

        // FADING OUT
        while (timer < fadeDuration)
        {
            float alpha = timer / fadeDuration;
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            canvasGroup.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        canvasGroup.alpha = 1f;

        // Cargar escena
        yield return SceneManager.LoadSceneAsync(targetScene);

        // FADING IN
        timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = 1f - (timer / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            canvasGroup.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        canvasGroup.alpha = 0f;

        Initiate.DoneFading();
        Destroy(gameObject);
    }
}
