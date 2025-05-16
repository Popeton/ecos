using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera; // Cámara hacia la que el objeto mirará

    private void Start()
    {
        // Si no se asigna una cámara, usar la cámara principal
        if (targetCamera == null) {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera != null) {
            // Hacer que el objeto mire directamente a la cámara
            transform.LookAt(targetCamera.transform);

            // Invertir la rotación en el eje Y si es necesario (si el objeto aparece al revés)
            transform.Rotate(0, 180, 0);
        }
    }
}

