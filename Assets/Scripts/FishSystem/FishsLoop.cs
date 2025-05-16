using UnityEngine;
using DG.Tweening;

public class FishsLoop : MonoBehaviour
{
    [SerializeField] private Vector3 centerPosition = new Vector3(0, 0, 0); // Centro del círculo
    [SerializeField] private float radius = 5f;  // Radio del círculo
    [SerializeField] private float speed = 5f;   // Velocidad del recorrido

    private void Start()
    {
        MoveInCircle();
    }
    

    void MoveInCircle()
    {
        // Creamos un conjunto de puntos para formar un círculo en la posición deseada
        Vector3[] path = new Vector3[10]; // Más puntos = círculo más suave
        for (int i = 0; i < path.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / path.Length; // Ángulo en radianes
            path[i] = centerPosition + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        // Configuramos el movimiento en loop
        transform.DOPath(path, speed, PathType.CatmullRom) // Movimiento circular
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetLookAt(0.01f);
    }
}