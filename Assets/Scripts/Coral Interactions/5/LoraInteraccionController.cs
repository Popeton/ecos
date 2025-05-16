using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoraInteraccionController : MonoBehaviour
{
   // [SerializeField] CameraFocus camRef;
    [SerializeField] FadeInOutUI uiRef;
    [SerializeField] SwitchFishPaths switchPathRef;
    [SerializeField] FishPathFollowing fishPathRef;

    [SerializeField] float timeToReturn;
    bool onInteraction;

    [SerializeField] VRInteractionHandler interactionHandler;
    private XRSimpleInteractable loraInteractable;


    private void Awake()
    {
        loraInteractable = GetComponent<XRSimpleInteractable>();
    }

    private void Start()
    {
        if (loraInteractable != null && interactionHandler != null) {
            // Añadir la interacción con el cangrejo al VRInteractionHandler
            interactionHandler.AddInteractable(loraInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if(interactable==loraInteractable && !onInteraction)
        StartCoroutine(LoraInteraction());
    }


    IEnumerator LoraInteraction()
    {
        onInteraction = true;
        //camRef.FocusOnTarget();
        switchPathRef.SwitchPath();
        fishPathRef.loop = false;
        yield return new WaitForSeconds(1f);
        uiRef.StartUIVisibility();
        yield return new WaitForSeconds(timeToReturn);
        RestoOriginalState();
    }

    void RestoOriginalState()
    {
        switchPathRef.RestoreOriginalPath();
        fishPathRef.loop = true;
        onInteraction = false;
    }

    private void OnDestroy()
    {
        interactionHandler.OnInteractionStarted -= StartInteraction;
    }

}
