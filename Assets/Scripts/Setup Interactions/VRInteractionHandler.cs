    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.Interaction.Toolkit;
    using UnityEngine.XR.Interaction.Toolkit.Interactables;

    public class VRInteractionHandler : MonoBehaviour
    {
        [SerializeField] private InputActionProperty triggerButtonAction; // Bot�n trigger en VR

        // Clase para manejar individualmente cada interacci�n
        private class InteractionState
        {
            public XRSimpleInteractable interactable;
            public bool hasInteracted; // Para controlar si ya se ha interactuado
            public InteractionState(XRSimpleInteractable interactable)
            {
                this.interactable = interactable;
                hasInteracted = false;
            }
        }

        // Lista de objetos interactuables con su estado de interacci�n
        private List<InteractionState> interactableStates = new List<InteractionState>();

        // Lista de interactuables a eliminar despu�s de la interacci�n
        private List<InteractionState> interactablesToRemove = new List<InteractionState>();

        // Delegado para suscribirse a la interacci�n
        public delegate void InteractionStartedHandler(XRSimpleInteractable interactable);
        public event InteractionStartedHandler OnInteractionStarted;

        // M�todo para agregar un nuevo interactuable
        public void AddInteractable(XRSimpleInteractable interactableObject)
        {
            if (!interactableStates.Exists(i => i.interactable == interactableObject))
            {
                var interactionState = new InteractionState(interactableObject);
                interactableStates.Add(interactionState);
                interactableObject.selectEntered.AddListener((args) => StartInteraction(interactionState));
                interactableObject.selectExited.AddListener((args) => EndInteraction(interactionState));
            }
        }

        // M�todo para remover un interactuable
        public void RemoveInteractable(XRSimpleInteractable interactableObject)
        {
            var interactionState = interactableStates.Find(i => i.interactable == interactableObject);
            if (interactionState != null)
            {
                interactablesToRemove.Add(interactionState); // A�adir a la lista de eliminaci�n, no directamente en el bucle
            }
        }

        void Update()
        {
            // Recorrer los interactuables activos y verificar la interacci�n
            foreach (var interactionState in interactableStates)
            {
                if (interactionState.interactable != null &&
                    interactionState.interactable.isSelected &&
                    triggerButtonAction.action.ReadValue<float>() > 0.1f &&
                    !interactionState.hasInteracted) // Solo si no ha interactuado a�n
                {
                    interactionState.hasInteracted = true; // Marcar como interactuado
                    OnInteractionStarted?.Invoke(interactionState.interactable); // Notificar interacci�n
                }
            }

            // Remover interactuables despu�s del bucle
            foreach (var interactableToRemove in interactablesToRemove)
            {
                if (interactableToRemove != null)
                {
                    interactableToRemove.interactable.selectEntered.RemoveListener((args) => StartInteraction(interactableToRemove));
                    interactableToRemove.interactable.selectExited.RemoveListener((args) => EndInteraction(interactableToRemove));
                    interactableStates.Remove(interactableToRemove); // Remover de la lista principal
                }
            }

            // Limpiar la lista de interactuables a remover
            interactablesToRemove.Clear();
        }



        // M�todo para iniciar la interacci�n
        private void StartInteraction(InteractionState interactionState)
        {
            if (!interactionState.hasInteracted)
            {
               if (interactionState.interactable.TryGetComponent<InteractableSound>(out var sound))
               {
                sound.PlayInteractionSound();
                }
            Debug.Log("Interacci�n iniciada con: " + interactionState.interactable.gameObject.name);
            }
        }

        // M�todo para finalizar la interacci�n
        private void EndInteraction(InteractionState interactionState)
        {
            if (interactionState.hasInteracted)
            {
                interactionState.hasInteracted = false; // Permitir que la interacci�n se vuelva a iniciar m�s tarde
                Debug.Log("Interacci�n terminada con: " + interactionState.interactable.gameObject.name);
            }
        }

        // Limpieza de todas las interacciones al destruir el objeto
        private void OnDestroy()
        {
            foreach (var interactionState in interactableStates)
            {
                if (interactionState.interactable != null)
                {
                    interactionState.interactable.selectEntered.RemoveListener((args) => StartInteraction(interactionState));
                    interactionState.interactable.selectExited.RemoveListener((args) => EndInteraction(interactionState));
                }
            }
            interactableStates.Clear();
        }
    }
