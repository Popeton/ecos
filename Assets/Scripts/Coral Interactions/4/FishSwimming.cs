using UnityEngine;

public class FishSwimming : MonoBehaviour
{
    public float sphereRadius = 10f; // Radio de la esfera en la que los peces nadarán
    public float speed = 2f; // Velocidad de los peces
    public float rotationSpeed = 1f; // Velocidad a la que cambian de dirección

    private Vector3 targetPosition;

    void Start()
    {
        // Establecer una posición inicial aleatoria dentro de la esfera
        transform.position = Random.insideUnitSphere * sphereRadius;

        // Generar un primer destino aleatorio
        SetNewTargetPosition();
    }

    void Update()
    {
        // Mover el pez hacia el objetivo actual
        MoveTowardsTarget();

        // Si el pez está cerca del objetivo, generar uno nuevo
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f) {
            SetNewTargetPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        // Calcular la dirección hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Suavizar la rotación hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mover al pez hacia adelante
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SetNewTargetPosition()
    {
        // Generar una nueva posición aleatoria dentro de la esfera
        targetPosition = Random.insideUnitSphere * sphereRadius;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar la esfera en la vista de editor para visualizar el área de movimiento
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
    }
}
