using UnityEngine;
using System.Collections;

public class SwitchObjects : MonoBehaviour
{
    public GameObject obj1; // Primer objeto hijo
    public GameObject obj2; // Segundo objeto hijo
    public float delayTime = 2f; // Tiempo de espera en segundos antes de cambiar el estado

    

    public void StartSwitch()
    {
        // Llamamos a la corutina para ejecutar la transición después de un tiempo
        StartCoroutine(WaitAndSwitchObjects());
    }

    private IEnumerator WaitAndSwitchObjects()
    {
        // Esperar por el tiempo especificado
        yield return new WaitForSeconds(delayTime);

        // Cambiar el estado de activación de ambos objetos
        obj1.SetActive(!obj1.activeSelf);
        obj2.SetActive(!obj2.activeSelf);
    }
}