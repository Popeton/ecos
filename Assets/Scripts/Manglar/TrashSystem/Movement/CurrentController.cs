using UnityEngine;

public class CurrentController : MonoBehaviour
{
    public bool isChanging; // Booleano para determinar si la corriente cambia o es constante
    public float changeInterval = 2f; // Intervalo para cambiar la dirección de la corriente
    public Vector3 constantCurrentDirection; // Dirección constante de la corriente (definida por el jugador)

    public float currentStrength = 1f; // Fuerza de la corriente (cuánto influye en el movimiento del objeto)
    public float directionSmoothness = 0.2f; // Suavizado de la dirección de la corriente

    private Vector3 currentDirection; // Dirección actual de la corriente
    private float timer; // Temporizador para cambiar la dirección de la corriente si 'isChanging' es true

    void Start()
    {
        // Inicializar la corriente en una dirección aleatoria si no se está cambiando
        if (!isChanging) {
            currentDirection = constantCurrentDirection.normalized; // Normalizar la dirección
        } else {
            ChangeCurrentDirection(); // Si cambia la corriente, inicializar con dirección aleatoria
        }
    }

    void Update()
    {
        if (isChanging) {
            // Si la corriente cambia, actualizar la dirección en intervalos
            timer += Time.deltaTime;
            if (timer > changeInterval) {
                ChangeCurrentDirection();
                timer = 0f;
            }
        }
        // Si no se está cambiando, la corriente mantiene su dirección constante
    }

    public Vector3 GetCurrentDirection()
    {
        return currentDirection * currentStrength; // Se multiplica por la fuerza de la corriente
    }

    void ChangeCurrentDirection()
    {
        // Cambiar la dirección de la corriente aleatoriamente con suavizado
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 newDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;

        // Suavizar la transición de la dirección actual hacia la nueva dirección
        currentDirection = Vector3.Lerp(currentDirection, newDirection, directionSmoothness);
    }

    void OnDrawGizmos()
    {
        // Dibujar la dirección de la corriente en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentDirection.normalized * 2f);
    }
}



