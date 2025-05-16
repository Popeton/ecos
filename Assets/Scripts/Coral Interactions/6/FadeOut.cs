using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f; // Duración del fade out en segundos
    private Renderer objectRenderer;
    private Color objectColor;
    private float fadeSpeed;

    private void Start()
    {
        // Obtén el Renderer del objeto para manipular su material
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null) {
            objectColor = objectRenderer.material.color;
            fadeSpeed = 1f / fadeDuration;
        } else {
            Debug.LogError("No se encontró un Renderer en el objeto.");
        }

    }

    public void StartFadeOut()
    {
        if (objectRenderer != null) {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        float alpha = objectColor.a;

        while (alpha > 0f) {
            alpha -= Time.deltaTime * fadeSpeed;
            objectColor.a = Mathf.Clamp01(alpha);
            objectRenderer.material.color = objectColor;
            yield return null;
        }

        // Desactiva el objeto una vez que se haya desvanecido
        gameObject.SetActive(false);
    }
}

