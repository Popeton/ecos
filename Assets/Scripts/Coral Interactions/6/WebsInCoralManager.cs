using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WebsInCoralManager : MonoBehaviour
{
    public List<GameObject> webs;
    
    [SerializeField] VRInteractionHandler interactionHandler;
    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject web in webs) {
            XRSimpleInteractable interactable = web.GetComponent<XRSimpleInteractable>();
            if (interactable != null) {
                interactionHandler.AddInteractable(interactable);
            } else {
                Debug.LogWarning($"El objeto {web.name} no tiene un componente XRSimpleInteractable.");
            }
            interactionHandler.OnInteractionStarted += web.GetComponent<WebInCoralController>().StartWebInteraction;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject web in webs) {

            interactionHandler.OnInteractionStarted -= web.GetComponent<WebInCoralController>().StartWebInteraction;
        }
    }
}
