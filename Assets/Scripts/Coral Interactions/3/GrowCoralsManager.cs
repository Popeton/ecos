using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrowCoralsManager : MonoBehaviour
{
    public List<GameObject> seeds;
    private List<GrowCoralController> seedInteraction;
    [SerializeField] VRInteractionHandler interactionHandler;

    void Start()
    {
        
    foreach(GameObject seed in seeds) {
            XRSimpleInteractable interactable = seed.GetComponent<XRSimpleInteractable>();
            if (interactable != null) {
                interactionHandler.AddInteractable(interactable);
            } else {
                Debug.LogWarning($"El objeto {seed.name} no tiene un componente XRSimpleInteractable.");
            }
            interactionHandler.OnInteractionStarted += seed.GetComponent<GrowCoralController>().StartCoral;
        }
        
    }

    private void OnDestroy()
    {
        foreach (GameObject seed in seeds)
        {

            interactionHandler.OnInteractionStarted -= seed.GetComponent<GrowCoralController>().StartCoral;
        }
    }
}

