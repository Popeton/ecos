using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VrInteractionsParameter : MonoBehaviour
{
    [System.Serializable]
    public class InteractionParameter
    {
        public XRSimpleInteractable interactable; // El objeto interactivo
        public StudioGlobalParameterTrigger parameterTrigger; // El par�metro de FMOD que controlar�
    }

    [SerializeField] private List<InteractionParameter> interactionParameters = new List<InteractionParameter>();
    public delegate void InteractionStartedHandler(int index);
    public event InteractionStartedHandler OnInteractionStarted;

    // M�todo para agregar un nuevo interactuable
    public void AddInteractable(InteractionParameter interactionParameter)
    {
        if (!interactionParameters.Contains(interactionParameter))
        {
            interactionParameters.Add(interactionParameter);
            interactionParameter.interactable.selectEntered.AddListener((args) => OnInteraction(args, interactionParameter));
        }
    }

    // M�todo para remover un interactuable
    public void RemoveInteractable(InteractionParameter interactionParameter)
    {
        if (interactionParameters.Contains(interactionParameter))
        {
            interactionParameter.interactable.selectEntered.RemoveAllListeners();
            interactionParameters.Remove(interactionParameter);
        }
    }

    // M�todo invocado cuando se interact�a con un objeto
    private void OnInteraction(SelectEnterEventArgs args, InteractionParameter interactionParameter)
    {
        interactionParameter.parameterTrigger.TriggerParameters();

        // Buscar el �ndice del par�metro en la lista y notificar el evento
        int index = interactionParameters.IndexOf(interactionParameter);
        OnInteractionStarted?.Invoke(index);
    }

    private void OnDestroy()
    {
        // Limpiar las suscripciones al destruir el objeto
        foreach (var interactionParam in interactionParameters)
        {
            if (interactionParam.interactable != null)
            {
                interactionParam.interactable.selectEntered.RemoveAllListeners();
            }
        }
    }
}
