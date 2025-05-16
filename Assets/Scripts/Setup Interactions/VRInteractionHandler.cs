    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.Interaction.Toolkit;
    using UnityEngine.XR.Interaction.Toolkit.Interactables;

    public class VRInteractionHandler : MonoBehaviour
    {
        [SerializeField] private InputActionProperty triggerButtonAction; // Botón trigger en VR

        // Clase para manejar individualmente cada interacción
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

        // Lista de objetos interactuables con su estado de interacción
        private List<InteractionState> interactableStates = new List<InteractionState>();

        // Lista de interactuables a eliminar después de la interacción
        private List<InteractionState> interactablesToRemove = new List<InteractionState>();

        // Delegado para suscribirse a la interacción
        public delegate void InteractionStartedHandler(XRSimpleInteractable interactable);
        public event InteractionStartedHandler OnInteractionStarted;

        // Método para agregar un nuevo interactuable
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

        // Método para remover un interactuable
        public void RemoveInteractable(XRSimpleInteractable interactableObject)
        {
            var interactionState = interactableStates.Find(i => i.interactable == interactableObject);
            if (interactionState != null)
            {
                interactablesToRemove.Add(interactionState); // Añadir a la lista de eliminación, no directamente en el bucle
            }
        }

        void Update()
        {
            // Recorrer los interactuables activos y verificar la interacción
            foreach (var interactionState in interactableStates)
            {
                if (interactionState.interactable != null &&
                    interactionState.interactable.isSelected &&
                    triggerButtonAction.action.ReadValue<float>() > 0.1f &&
                    !interactionState.hasInteracted) // Solo si no ha interactuado aún
                {
                    interactionState.hasInteracted = true; // Marcar como interactuado
                    OnInteractionStarted?.Invoke(interactionState.interactable); // Notificar interacción
                }
            }

            // Remover interactuables después del bucle
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



        // Método para iniciar la interacción
        private void StartInteraction(InteractionState interactionState)
        {
            if (!interactionState.hasInteracted)
            {
               if (interactionState.interactable.TryGetComponent<InteractableSound>(out var sound))
               {
                sound.PlayInteractionSound();
                }
            Debug.Log("Interacción iniciada con: " + interactionState.interactable.gameObject.name);
            }
        }

        // Método para finalizar la interacción
        private void EndInteraction(InteractionState interactionState)
        {
            if (interactionState.hasInteracted)
            {
                interactionState.hasInteracted = false; // Permitir que la interacción se vuelva a iniciar más tarde
                Debug.Log("Interacción terminada con: " + interactionState.interactable.gameObject.name);
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
