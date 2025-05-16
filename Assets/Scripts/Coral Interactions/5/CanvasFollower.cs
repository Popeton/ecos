using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public Transform target; // Objeto a seguir
    public Vector3 offset = new Vector3(0, 2, 0); // Posición relativa respecto al objeto
    public Transform player;
    void Update()
    {
        if (target != null) {
            transform.position = target.position + offset;
            transform.LookAt(player); // Opcional: Hace que el Canvas mire siempre a la cámara
        }
    }
}
