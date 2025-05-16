using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public Transform centerTransform; // Transform que define el centro del �rea
    public float areaRadius = 5f; // Radio del c�rculo donde el objeto se mover�
    public float speed = 2f; // Velocidad constante del objeto
    public float directionChangeInterval = 2f; // Intervalo para cambiar la direcci�n aleatoria
    public float directionChangeSmoothness = 0.2f; // Factor de suavizado para el cambio de direcci�n

    private Rigidbody rb; // Referencia al Rigidbody
    private Vector3 movementDirection; // Direcci�n actual de movimiento
    private Vector3 targetDirection; // Direcci�n objetivo a la que se desea mover
    private float timer; // Temporizador para el cambio de direcci�n

    void Start()
    {
        // Obtener el Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivar la gravedad si no es necesaria
        rb.constraints = RigidbodyConstraints.FreezePositionY; // Mantener el objeto en el plano XZ

        // Elegir una direcci�n inicial aleatoria
        ChooseNewDirection();
    }

    void FixedUpdate()
    {
        if (centerTransform == null) {
            Debug.LogWarning("Falta la referencia al 'centerTransform'.");
            return;
        }

        // Interpolaci�n suave entre la direcci�n actual y la nueva direcci�n
        movementDirection = Vector3.Lerp(movementDirection, targetDirection, directionChangeSmoothness * Time.fixedDeltaTime);

        // Normalizar la direcci�n y calcular la velocidad constante
        Vector3 movementVelocity = movementDirection.normalized * speed;

        // Asignar la velocidad al Rigidbody
        rb.velocity = movementVelocity;

        // Comprobar si el objeto est� fuera del �rea permitida
        Vector3 centerPoint = centerTransform.position;
        Vector3 offset = transform.position - centerPoint;
        offset.y = 0f; // Ignorar la componente Y para solo trabajar en XZ
        float distanceFromCenter = offset.magnitude;

        if (distanceFromCenter >= areaRadius) {
            // Cambiar la direcci�n hacia el interior del c�rculo
            Vector3 directionToCenter = offset.normalized;
            targetDirection = -directionToCenter; // Redirigir el movimiento hacia adentro

            // Limitar la posici�n al borde del c�rculo
            Vector3 clampedPosition = centerPoint + directionToCenter * areaRadius;
            transform.position = clampedPosition;
        }

        // Controlar el tiempo para cambiar la direcci�n
        timer += Time.fixedDeltaTime;
        if (timer > directionChangeInterval) {
            ChooseNewDirection();
            timer = 0f;
        }
    }

    void ChooseNewDirection()
    {
        // Elegir una direcci�n aleatoria en el plano XZ
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        targetDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    void OnDrawGizmos()
    {
        if (centerTransform != null) {
            // Dibujar el �rea del c�rculo en el editor en 3D (solo XZ)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(centerTransform.position.x, transform.position.y, centerTransform.position.z), areaRadius);
        }
    }
}




