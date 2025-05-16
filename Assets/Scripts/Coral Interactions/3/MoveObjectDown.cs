using UnityEngine;
using System.Collections;

public class MoveObjectDown : MonoBehaviour
{
    public float moveDistance = 2f; // Distancia que se moverá el objeto hacia abajo
    public float moveSpeed = 2f; // Velocidad de movimiento suave

    private Vector3 targetPosition; // Posición objetivo a donde se moverá el objeto


   


    // Función que puedes llamar para mover el objeto hacia abajo
    public void MoveDown()
    {
        // Calculamos la nueva posición hacia abajo
        targetPosition = transform.position - new Vector3(0, moveDistance, 0);

        // Iniciamos la corutina para mover el objeto suavemente
        StartCoroutine(SmoothMove());
    }

    private IEnumerator SmoothMove()
    {
        // Mientras el objeto no haya alcanzado la posición objetivo
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            // Mover el objeto suavemente hacia la posición objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Esperamos un frame
            yield return null;
        }

        // Aseguramos que el objeto llegue exactamente a la posición
        transform.position = targetPosition;
    }
}

