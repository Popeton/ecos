using System.Collections;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform target; // Objeto al que la cámara se enfocará
    public float focusDuration = 2f; // Tiempo que la cámara permanecerá enfocada
    public float transitionSpeed = 2f; // Velocidad de la transición
    public float zoomedFOV = 30f; // FOV al hacer zoom (menor valor = más zoom)

    private float originalFOV; // FOV original de la cámara
    private Quaternion originalRotation; // Rotación original de la cámara
    private bool isFocusing = false; // Estado del enfoque

    private Camera cam; // Referencia a la cámara

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("Este script debe estar en un objeto con una componente Camera.");
            return;
        }

        // Guardar la configuración original
        originalFOV = cam.fieldOfView;
        originalRotation = transform.rotation;

    }

    public void FocusOnTarget()
    {
        if (!isFocusing) // Evitar múltiples transiciones simultáneas
        {
            StartCoroutine(FocusCoroutine());
        }
    }

    private IEnumerator FocusCoroutine()
    {
        isFocusing = true;

        // Transición hacia el objetivo (rotación dinámica + cambio de FOV)
        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // Rotación suavizada hacia el objetivo
            if (target != null) {
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime);
            }

            // Suavizar el FOV (zoom)
            cam.fieldOfView = Mathf.Lerp(originalFOV, zoomedFOV, elapsedTime);

            yield return null;
        }

        // Mantener el enfoque mientras el target se mueve
        float focusTime = 0f;
        while (focusTime < focusDuration) {
            focusTime += Time.deltaTime;

            // Actualizar la rotación en tiempo real para seguir al target
            if (target != null) {
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
            }

            yield return null;
        }

        // Transición de regreso al estado original
        elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // Rotación suavizada de regreso
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, elapsedTime);

            // Restaurar el FOV suavemente
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalFOV, elapsedTime);

            yield return null;
        }

        // Restaurar completamente el estado original
        transform.rotation = originalRotation;
        cam.fieldOfView = originalFOV;

        isFocusing = false;
    }
}