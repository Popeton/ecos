using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;

    [Header("Fish Settings")]
    public GameObject[] fishPrefabs; // Arreglo de diferentes tipos de peces
    public GameObject[] interactiveFishPrefabs; // Peces interactuables únicos ya configurados
    public int numFish = 20; // Total de peces en la escena
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos = Vector3.zero;

    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    [SerializeField]
    private Vector2 yPositionRange = new Vector2(-1.5f, 0f);
    void Start()
    {
        // Inicializar el arreglo de peces
        allFish = new GameObject[numFish];

        // Configurar los peces interactivos existentes
        SetupExistingInteractiveFish(interactiveFishPrefabs);

        // Instanciar peces generales si hay espacio restante
        int remainingFish = numFish - interactiveFishPrefabs.Length;
        if (remainingFish > 0)
        {
            SetupNewFish(remainingFish);
        }

        FM = this;
        goalPos = this.transform.position;
    }

    void Update()
    {
        if (Random.Range(0, 100) < 10)
        {
            goalPos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(yPositionRange.x, yPositionRange.y),
                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }

    /// <summary>
    /// Configura los peces interactivos existentes en el sistema de flocking.
    /// </summary>
    /// <param name="interactiveFish">Arreglo de peces interactivos existentes</param>
    private void SetupExistingInteractiveFish(GameObject[] interactiveFish)
    {
        int count = Mathf.Min(interactiveFish.Length, numFish);

        for (int i = 0; i < count; i++)
        {
            if (interactiveFish[i] != null)
            {
                allFish[i] = interactiveFish[i];
                allFish[i].SetActive(true); // Asegurar que estén activos
            }
            else
            {
                Debug.LogWarning($"El pez interactivo en la posición {i} es nulo. Asegúrate de asignarlo correctamente en el inspector.");
            }
        }
    }
    /// <summary>
    /// Instancia nuevos peces para llenar el grupo si hay espacio disponible.
    /// </summary>
    /// <param name="remainingFish">Cantidad de peces restantes por instanciar</param>
    private void SetupNewFish(int remainingFish)
    {
        int currentCount = interactiveFishPrefabs.Length;

        for (int i = 0; i < remainingFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(yPositionRange.x, yPositionRange.y),
                Random.Range(-swimLimits.z, swimLimits.z));

            int randomIndex = Random.Range(0, fishPrefabs.Length);
            GameObject newFish = Instantiate(fishPrefabs[randomIndex], pos, Quaternion.identity, this.transform); // Establece el padre
            allFish[currentCount + i] = newFish;
            newFish.SetActive(true);
        }
    }
}
