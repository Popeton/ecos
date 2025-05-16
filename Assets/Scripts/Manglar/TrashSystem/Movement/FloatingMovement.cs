using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public Transform centerTransform; // Transform que define el centro del área
    public float areaRadius = 5f; // Radio del círculo donde el objeto se moverá
    public float speed = 2f; // Velocidad constante del objeto
    public float directionChangeInterval = 2f; // Intervalo para cambiar la dirección aleatoria
    public float directionChangeSmoothness = 0.2f; // Factor de suavizado para el cambio de dirección

    private Rigidbody rb; // Referencia al Rigidbody
    private Vector3 movementDirection; // Dirección actual de movimiento
    private Vector3 targetDirection; // Dirección objetivo a la que se desea mover
    private float timer; // Temporizador para el cambio de dirección

    void Start()
    {
        // Obtener el Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivar la gravedad si no es necesaria
        rb.constraints = RigidbodyConstraints.FreezePositionY; // Mantener el objeto en el plano XZ

        // Elegir una dirección inicial aleatoria
        ChooseNewDirection();
    }

    void FixedUpdate()
    {
        if (centerTransform == null) {
            Debug.LogWarning("Falta la referencia al 'centerTransform'.");
            return;
        }

        // Interpolación suave entre la dirección actual y la nueva dirección
        movementDirection = Vector3.Lerp(movementDirection, targetDirection, directionChangeSmoothness * Time.fixedDeltaTime);

        // Normalizar la dirección y calcular la velocidad constante
        Vector3 movementVelocity = movementDirection.normalized * speed;

        // Asignar la velocidad al Rigidbody
        rb.velocity = movementVelocity;

        // Comprobar si el objeto está fuera del área permitida
        Vector3 centerPoint = centerTransform.position;
        Vector3 offset = transform.position - centerPoint;
        offset.y = 0f; // Ignorar la componente Y para solo trabajar en XZ
        float distanceFromCenter = offset.magnitude;

        if (distanceFromCenter >= areaRadius) {
            // Cambiar la dirección hacia el interior del círculo
            Vector3 directionToCenter = offset.normalized;
            targetDirection = -directionToCenter; // Redirigir el movimiento hacia adentro

            // Limitar la posición al borde del círculo
            Vector3 clampedPosition = centerPoint + directionToCenter * areaRadius;
            transform.position = clampedPosition;
        }

        // Controlar el tiempo para cambiar la dirección
        timer += Time.fixedDeltaTime;
        if (timer > directionChangeInterval) {
            ChooseNewDirection();
            timer = 0f;
        }
    }

    void ChooseNewDirection()
    {
        // Elegir una dirección aleatoria en el plano XZ
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        targetDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    void OnDrawGizmos()
    {
        if (centerTransform != null) {
            // Dibujar el área del círculo en el editor en 3D (solo XZ)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(centerTransform.position.x, transform.position.y, centerTransform.position.z), areaRadius);
        }
    }
}




