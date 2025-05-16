using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectsManager : MonoBehaviour
{
    public List<GameObject> objectsToActivate; // Lista de objetos que serán activados
    public float timeBeforeSwitch = 3f; // Tiempo antes de cambiar a movimiento flotante
    private bool objectsActivated = false; // Para evitar que se activen más de una vez

    
    //variables de testeo
    public CustomEvent evento;

    // Función que se puede llamar para activar los objetos y empezar el temporizador
    public void ActivateObjects()
    {
        if (!objectsActivated) {
            // Activar los objetos
            foreach (var obj in objectsToActivate) {
                if (obj != null) {
                    obj.SetActive(true); // Activar el objeto
                }
            }

            objectsActivated = true; // Evitar que se active más de una vez

            // Iniciar la corrutina para cambiar a flotante después de un tiempo
            StartCoroutine(ActivateFloatingMovementAfterTime());
        }
    }

    // Corutina para esperar un tiempo y luego cambiar los scripts
    private IEnumerator ActivateFloatingMovementAfterTime()
    {
        // Esperar el tiempo antes de cambiar
        yield return new WaitForSeconds(timeBeforeSwitch);

        // Recorrer la lista de objetos y hacer el cambio
        foreach (var obj in objectsToActivate) {
            if (obj != null) {
                // Desactivar SimpleBounce
                SimpleBounce simpleBounceScript = obj.GetComponent<SimpleBounce>();
                if (simpleBounceScript != null) {
                    simpleBounceScript.enabled = false;
                }

                // Activar FloatingMovement o FloatingMovementWithCurrents
                FloatingMovement floatingMovementScript = obj.GetComponent<FloatingMovement>();
                if (floatingMovementScript != null) {
                    floatingMovementScript.enabled = true; // Activar sin corrientes
                } else {
                    FloatingMovementWithCurrents floatingMovementWithCurrentsScript = obj.GetComponent<FloatingMovementWithCurrents>();
                    if (floatingMovementWithCurrentsScript != null) {
                        floatingMovementWithCurrentsScript.enabled = true; // Activar con corrientes
                    }
                }
            }
        }
    }


    //--------------------------------------------Espacio de Testeo


    private void Start()
    {
        evento.GEvent += ActivateObjects;
    }

    private void OnDestroy()
    {
        evento.GEvent -= ActivateObjects;
    }
    private void OnDisable()
    {
        evento.GEvent -= ActivateObjects;
    }
}

