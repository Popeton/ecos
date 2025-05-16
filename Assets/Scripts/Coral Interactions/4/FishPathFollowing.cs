using System.Collections.Generic;
using UnityEngine;

public class FishPathFollowing : MonoBehaviour
{
    public List<Transform> waypoints; // Lista de puntos a seguir
    public float speed = 2f; // Velocidad del pez
    public float rotationSpeed = 2f; // Velocidad de rotaci�n del pez
    public bool loop = true; // Si el pez debe recorrer la lista en bucle o detenerse en el �ltimo punto
    public float decelerationDistance = 2f; // Distancia desde el �ltimo punto donde el pez empieza a desacelerar

    private int currentWaypointIndex = 0; // �ndice del punto actual
    private bool isMoving = true; // Verifica si el pez sigue movi�ndose

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0) {
            Debug.LogWarning("El pez est� detenido o no hay puntos en la lista de waypoints.");
            return;
        }

        // Verificar si debe reiniciar el movimiento cuando el bucle est� activado
        if (!isMoving && loop) {
            RestartPatrol();
        }

        if (isMoving) {
            // Mover el pez hacia el punto actual
            MoveTowardsWaypoint();
        }
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calcular la direcci�n hacia el punto objetivo
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Suavizar la rotaci�n hacia el punto objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Ajustar la velocidad si el pez se acerca al �ltimo punto
        float currentSpeed = speed;
        if (!loop && currentWaypointIndex == waypoints.Count - 1) {
            float distanceToLastWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
            if (distanceToLastWaypoint < decelerationDistance) {
                currentSpeed = Mathf.Lerp(0, speed, distanceToLastWaypoint / decelerationDistance);
            }
        }

        // Mover al pez hacia adelante
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Comprobar si el pez ha llegado cerca del punto objetivo
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f) {
            // Pasar al siguiente punto de la lista
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count) {
                if (loop) {
                    // Volver al primer punto si el bucle est� activado
                    RestartPatrol();
                } else {
                    // Detener el movimiento si no hay bucle
                    isMoving = false;
                }
            }
        }
    }

    public void RestartPatrol()
    {
        currentWaypointIndex = 0;
        isMoving = true;
    }

    private void OnDrawGizmos()
    {
        // Dibujar l�neas entre los puntos de la lista para visualizaci�n
        if (waypoints != null && waypoints.Count > 1) {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Count - 1; i++) {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }

            // Dibujar l�nea del �ltimo al primero para cerrar el circuito (opcional)
            if (loop) {
                Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
    }
}
