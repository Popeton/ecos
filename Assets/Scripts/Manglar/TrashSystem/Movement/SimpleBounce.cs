using UnityEngine;

public class SimpleBounce : MonoBehaviour
{
    public Transform surfacePoint; // La posici�n de la superficie
    public AnimationCurve riseCurve; // Curva para el movimiento de subida
    public AnimationCurve bounceCurve; // Curva para los rebotes
    public float floatSpeed = 2f; // Velocidad de subida hacia la superficie
    public float bounceForce = 5f; // Fuerza inicial del rebote cuando llega a la superficie
    public float startHeightOffset = -2f; // La altura inicial debajo de la superficie para empezar
    public float bounceDuration = 1f; // Duraci�n de la animaci�n de rebote

    private Rigidbody rb; // El Rigidbody del objeto
    private float startTime; // Tiempo de inicio para la animaci�n
    private bool isRising = true; // Indicador para saber si el objeto est� subiendo
    private bool hasReachedSurface = false; // Indicador para saber si el objeto ha alcanzado la superficie
    private Vector3 initialPosition; // Posici�n inicial del objeto

    void Start()
    {
        // Obtener el Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivar la gravedad inicialmente
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; // Restringir el movimiento en el eje X y Z

        // Guardar la posici�n inicial
        initialPosition = transform.position;

        // Colocamos el objeto debajo de la superficie al inicio
        transform.position = new Vector3(transform.position.x, surfacePoint.position.y + startHeightOffset, transform.position.z);

        // Iniciar la animaci�n de subida
        startTime = Time.time;
    }

    void Update()
    {
        if (isRising) {
            // Controlar la subida del objeto usando la curva de subida
            float elapsedTime = (Time.time - startTime) * floatSpeed;
            float t = Mathf.Clamp01(elapsedTime / bounceDuration); // Normalizar el tiempo de la animaci�n entre 0 y 1

            // Usar la curva para obtener el valor de la subida
            float curveValue = riseCurve.Evaluate(t);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(initialPosition.y, surfacePoint.position.y, curveValue), transform.position.z);

            // Verificar si alcanz� la superficie
            if (t >= 1f) {
                // Cuando el objeto llega a la superficie, inicia el rebote
                hasReachedSurface = true;
                isRising = false;
                startTime = Time.time; // Reiniciar el tiempo para el rebote
            }
        } else if (hasReachedSurface) {
            // Controlar el rebote usando la curva de rebote
            float elapsedTime = (Time.time - startTime);
            float t = Mathf.Clamp01(elapsedTime / bounceDuration); // Normalizar el tiempo de la animaci�n entre 0 y 1

            // Usar la curva de rebote para el movimiento
            float bounceHeight = bounceCurve.Evaluate(t) * bounceForce;
            transform.position = new Vector3(transform.position.x, surfacePoint.position.y + bounceHeight, transform.position.z);

            // Detener el rebote cuando el tiempo haya pasado
            if (t >= 1f) {
                // Detener el objeto y asegurarse de que se quede en la superficie
                transform.position = new Vector3(transform.position.x, surfacePoint.position.y, transform.position.z);
                rb.velocity = Vector3.zero; // Detener la velocidad
                hasReachedSurface = false; // No m�s rebotes
            }
        }
    }
}


