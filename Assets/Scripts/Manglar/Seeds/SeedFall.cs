using DG.Tweening;
using UnityEngine;

public class SeedFall : MonoBehaviour
{
    [SerializeField] private SeedGrowthManager seedGrowthManager; // Referencia al SeedGrowthManager
    [SerializeField] private AudioManager audioInstance;
   


    public void HandleInteraction()
    {
        // Llamamos al método de SeedGrowthManager para iniciar el crecimiento cuando la semilla es seleccionada
        seedGrowthManager.SelectSeed();
        audioInstance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, audioInstance.transform.position);
        // Remover la interacción después de que ocurra
        // interactionHandler.RemoveInteractable(interactable);
    }
}
