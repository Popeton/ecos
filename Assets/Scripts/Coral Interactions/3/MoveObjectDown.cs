using UnityEngine;
using System.Collections;

public class MoveObjectDown : MonoBehaviour
{
    public float moveDistance = 2f; // Distancia que se mover� el objeto hacia abajo
    public float moveSpeed = 2f; // Velocidad de movimiento suave

    private Vector3 targetPosition; // Posici�n objetivo a donde se mover� el objeto


   


    // Funci�n que puedes llamar para mover el objeto hacia abajo
    public void MoveDown()
    {
        // Calculamos la nueva posici�n hacia abajo
        targetPosition = transform.position - new Vector3(0, moveDistance, 0);

        // Iniciamos la corutina para mover el objeto suavemente
        StartCoroutine(SmoothMove());
    }

    private IEnumerator SmoothMove()
    {
        // Mientras el objeto no haya alcanzado la posici�n objetivo
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            // Mover el objeto suavemente hacia la posici�n objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Esperamos un frame
            yield return null;
        }

        // Aseguramos que el objeto llegue exactamente a la posici�n
        transform.position = targetPosition;
    }
}

