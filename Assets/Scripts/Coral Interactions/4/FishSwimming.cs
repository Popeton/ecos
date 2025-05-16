using UnityEngine;

public class FishSwimming : MonoBehaviour
{
    public float sphereRadius = 10f; // Radio de la esfera en la que los peces nadar�n
    public float speed = 2f; // Velocidad de los peces
    public float rotationSpeed = 1f; // Velocidad a la que cambian de direcci�n

    private Vector3 targetPosition;

    void Start()
    {
        // Establecer una posici�n inicial aleatoria dentro de la esfera
        transform.position = Random.insideUnitSphere * sphereRadius;

        // Generar un primer destino aleatorio
        SetNewTargetPosition();
    }

    void Update()
    {
        // Mover el pez hacia el objetivo actual
        MoveTowardsTarget();

        // Si el pez est� cerca del objetivo, generar uno nuevo
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f) {
            SetNewTargetPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        // Calcular la direcci�n hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Suavizar la rotaci�n hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mover al pez hacia adelante
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SetNewTargetPosition()
    {
        // Generar una nueva posici�n aleatoria dentro de la esfera
        targetPosition = Random.insideUnitSphere * sphereRadius;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar la esfera en la vista de editor para visualizar el �rea de movimiento
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
    }
}
