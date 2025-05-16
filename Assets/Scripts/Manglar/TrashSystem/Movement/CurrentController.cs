using UnityEngine;

public class CurrentController : MonoBehaviour
{
    public bool isChanging; // Booleano para determinar si la corriente cambia o es constante
    public float changeInterval = 2f; // Intervalo para cambiar la direcci�n de la corriente
    public Vector3 constantCurrentDirection; // Direcci�n constante de la corriente (definida por el jugador)

    public float currentStrength = 1f; // Fuerza de la corriente (cu�nto influye en el movimiento del objeto)
    public float directionSmoothness = 0.2f; // Suavizado de la direcci�n de la corriente

    private Vector3 currentDirection; // Direcci�n actual de la corriente
    private float timer; // Temporizador para cambiar la direcci�n de la corriente si 'isChanging' es true

    void Start()
    {
        // Inicializar la corriente en una direcci�n aleatoria si no se est� cambiando
        if (!isChanging) {
            currentDirection = constantCurrentDirection.normalized; // Normalizar la direcci�n
        } else {
            ChangeCurrentDirection(); // Si cambia la corriente, inicializar con direcci�n aleatoria
        }
    }

    void Update()
    {
        if (isChanging) {
            // Si la corriente cambia, actualizar la direcci�n en intervalos
            timer += Time.deltaTime;
            if (timer > changeInterval) {
                ChangeCurrentDirection();
                timer = 0f;
            }
        }
        // Si no se est� cambiando, la corriente mantiene su direcci�n constante
    }

    public Vector3 GetCurrentDirection()
    {
        return currentDirection * currentStrength; // Se multiplica por la fuerza de la corriente
    }

    void ChangeCurrentDirection()
    {
        // Cambiar la direcci�n de la corriente aleatoriamente con suavizado
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 newDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;

        // Suavizar la transici�n de la direcci�n actual hacia la nueva direcci�n
        currentDirection = Vector3.Lerp(currentDirection, newDirection, directionSmoothness);
    }

    void OnDrawGizmos()
    {
        // Dibujar la direcci�n de la corriente en el editor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentDirection.normalized * 2f);
    }
}



