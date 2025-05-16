using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CrabInteractions : MonoBehaviour
{
    [SerializeField] private GameObject crab; // El cangrejo
    [SerializeField] private VRInteractionHandler interactionHandler; // Referencia al VRInteractionHandler
    [SerializeField] private AudioManager audioInstance; // Audio Manager para reproducir los sonidos
    [SerializeField] private GameObject fisherman; // Pescador que aparece
   
  
    [SerializeField] private GameObject crabUi; // Bote con outline
    

    private bool hasGrabbed = false;
  

    private XRSimpleInteractable crabInteractable;

    void Start()
    {
        //ActivateFisherman();
        // Obtener el interactuable del cangrejo
        crabInteractable = crab.GetComponent<XRSimpleInteractable>();
        crabInteractable.enabled = true;
        crab.GetComponent<Collider>().enabled = true;
        this.GetComponent<Outlinable>().enabled = true; // Activar el outline del cangrejo

        if (crabInteractable != null && interactionHandler != null)
        {
            // Añadir la interacción con el cangrejo al VRInteractionHandler
            interactionHandler.AddInteractable(crabInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += OnCrabInteractionStarted;
        }
        else
        {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
    }

    // Método invocado cuando se interactúa con el cangrejo
    private void OnCrabInteractionStarted(XRSimpleInteractable interactable)
    {
        if (interactable == crabInteractable && !hasGrabbed)
        {
            StartCrabInteraction();
        }
    }

    private void StartCrabInteraction()
    {
        crabUi.SetActive(true);
       // crabOutline.enabled = false; // Desactivar el outline del cangrejo
        this.GetComponent<Outlinable>().enabled = false;
        crab.GetComponent<XRSimpleInteractable>().enabled = false;
        crab.GetComponent<Collider>().enabled = false;
        interactionHandler.RemoveInteractable(crabInteractable);
      
    }

    public void ActivateFisherman()
    {
        if (!fisherman.activeInHierarchy)
        {
            audioInstance.InitializeVoice(FmodEvents.instance.Interaccion4_Fase1, audioInstance.transform.position);
            fisherman.SetActive(true);
        }
    }

    public void CrabDestroy()
    {
       Destroy(this);
    }

    // Funciones para gestionar cada acción
   


    void OnDestroy()
    {
        // Desuscribirse para evitar errores cuando se destruya el objeto
        if (interactionHandler != null)
        {
            interactionHandler.OnInteractionStarted -= OnCrabInteractionStarted;
        }
    }
}
