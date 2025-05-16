using System.Collections;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform target; // Objeto al que la c�mara se enfocar�
    public float focusDuration = 2f; // Tiempo que la c�mara permanecer� enfocada
    public float transitionSpeed = 2f; // Velocidad de la transici�n
    public float zoomedFOV = 30f; // FOV al hacer zoom (menor valor = m�s zoom)

    private float originalFOV; // FOV original de la c�mara
    private Quaternion originalRotation; // Rotaci�n original de la c�mara
    private bool isFocusing = false; // Estado del enfoque

    private Camera cam; // Referencia a la c�mara

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("Este script debe estar en un objeto con una componente Camera.");
            return;
        }

        // Guardar la configuraci�n original
        originalFOV = cam.fieldOfView;
        originalRotation = transform.rotation;

    }

    public void FocusOnTarget()
    {
        if (!isFocusing) // Evitar m�ltiples transiciones simult�neas
        {
            StartCoroutine(FocusCoroutine());
        }
    }

    private IEnumerator FocusCoroutine()
    {
        isFocusing = true;

        // Transici�n hacia el objetivo (rotaci�n din�mica + cambio de FOV)
        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // Rotaci�n suavizada hacia el objetivo
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

            // Actualizar la rotaci�n en tiempo real para seguir al target
            if (target != null) {
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
            }

            yield return null;
        }

        // Transici�n de regreso al estado original
        elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // Rotaci�n suavizada de regreso
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