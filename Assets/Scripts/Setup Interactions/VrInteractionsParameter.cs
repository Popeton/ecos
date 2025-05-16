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
        public StudioGlobalParameterTrigger parameterTrigger; // El parámetro de FMOD que controlará
    }

    [SerializeField] private List<InteractionParameter> interactionParameters = new List<InteractionParameter>();
    public delegate void InteractionStartedHandler(int index);
    public event InteractionStartedHandler OnInteractionStarted;

    // Método para agregar un nuevo interactuable
    public void AddInteractable(InteractionParameter interactionParameter)
    {
        if (!interactionParameters.Contains(interactionParameter))
        {
            interactionParameters.Add(interactionParameter);
            interactionParameter.interactable.selectEntered.AddListener((args) => OnInteraction(args, interactionParameter));
        }
    }

    // Método para remover un interactuable
    public void RemoveInteractable(InteractionParameter interactionParameter)
    {
        if (interactionParameters.Contains(interactionParameter))
        {
            interactionParameter.interactable.selectEntered.RemoveAllListeners();
            interactionParameters.Remove(interactionParameter);
        }
    }

    // Método invocado cuando se interactúa con un objeto
    private void OnInteraction(SelectEnterEventArgs args, InteractionParameter interactionParameter)
    {
        interactionParameter.parameterTrigger.TriggerParameters();

        // Buscar el índice del parámetro en la lista y notificar el evento
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
