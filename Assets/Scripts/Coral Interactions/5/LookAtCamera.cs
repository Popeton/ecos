using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera; // C�mara hacia la que el objeto mirar�

    private void Start()
    {
        // Si no se asigna una c�mara, usar la c�mara principal
        if (targetCamera == null) {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera != null) {
            // Hacer que el objeto mire directamente a la c�mara
            transform.LookAt(targetCamera.transform);

            // Invertir la rotaci�n en el eje Y si es necesario (si el objeto aparece al rev�s)
            transform.Rotate(0, 180, 0);
        }
    }
}

