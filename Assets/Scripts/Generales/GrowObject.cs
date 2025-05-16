using UnityEngine;
using System.Collections;

public class GrowObject : MonoBehaviour
{
    public AnimationCurve growthCurve; // La curva que controla el crecimiento y reducci�n
    public float duration = 2f; // Tiempo total de la animaci�n
    private Vector3 originalScale; // Escala original del objeto

    [SerializeField] bool growOnAppear;
    [SerializeField] bool shrinkOnStart = true;
    [SerializeField] bool deactivateOnShrink = true;
    private void Awake()
    {
        // Guardamos la escala original del objeto
        originalScale = transform.localScale;

        if (shrinkOnStart)
        {
            transform.localScale = Vector3.zero;
        }
    }

    // Funci�n para iniciar el crecimiento
    public void Grow()
    {
        StartCoroutine(SmoothGrow());
    }

    // Funci�n para reducir el objeto a escala cero
    public void Shrink()
    {
        StartCoroutine(SmoothShrink());
    }

    private IEnumerator SmoothGrow()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float curveValue = growthCurve.Evaluate(timeElapsed / duration);
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, curveValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    private IEnumerator SmoothShrink()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float curveValue = growthCurve.Evaluate(timeElapsed / duration);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, curveValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        if (deactivateOnShrink)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (growOnAppear)
        {
            Grow(); // Se ejecuta autom�ticamente al activarse el objeto
        }
    }
}
