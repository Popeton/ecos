using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SeedGrowthManager : MonoBehaviour
{
    //[SerializeField] private GameObject[] seeds;  // Las semillas con las que se puede interactuar
    //[SerializeField] private GameObject[] mangroveStages;  // Fases del manglar
    //[SerializeField] private GameObject crab;
    //[SerializeField] private GameObject seedManager;

    //private bool hasSelectedSeed = false;
    //private int currentStage = 0;

    [Header("Configuración de Semillas")]
    [SerializeField] private GameObject[] seeds;  // Semillas interactuables
    [SerializeField] private GameObject seedManager;

    [Header("Configuración de Manglar")]
    [SerializeField] private GameObject mangroveModel; // Modelo único del manglar
    [SerializeField] private PlayableDirector growthTimeline; // Timeline de crecimiento
   // [SerializeField] private Outline mangroveOutline; // Componente Outline del modelo
    [SerializeField] private GameObject mangroves;

    [Header("Configuración Final")]
    [SerializeField] private GameObject crab; // Cangrejo a activar
    [SerializeField] private float animationDuration = 20f; // Duración de respaldo
    [SerializeField] private GameObject buttonSemilla;
    //[SerializeField] private GameObject audioinstance;
    private bool growthCompleted = false;


    void Start()
    {
        InitializeSystem();
    }

    private void InitializeSystem()
    {
        // Configuración inicial del modelo
        if (mangroveModel != null)
        {
            mangroveModel.SetActive(false);
           
        }
    }

    public void StartSeeds()
    {
        foreach (GameObject seed in seeds)
        {
            seed.SetActive(true); // Activa las semillas cuando sea el momento adecuado
        }
    }

    public void SelectSeed()
    {
        if (growthCompleted) return;

        // Desactiva todas las semillas
        foreach (GameObject seed in seeds)
        {
            seed.SetActive(false);
        }

        // Inicia el proceso de crecimiento
      StartCoroutine(GrowthProcess());
    }

    public void HandleInteraction()
    {
        // Llamamos al método de SeedGrowthManager para iniciar el crecimiento cuando la semilla es seleccionada
        SelectSeed();
        AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, AudioManager.instance.transform.position);
        // Remover la interacción después de que ocurra
        // interactionHandler.RemoveInteractable(interactable);
    }

    //public voi
    private IEnumerator GrowthProcess()
    {
        // Activa el modelo del manglar
        mangroveModel.SetActive(true);

      

        // Inicia la animación del timeline
        if (growthTimeline != null)
        {
            growthTimeline.Play();

            // Calcula el tiempo de espera
            float actualDuration = (float)growthTimeline.duration;
            yield return new WaitForSeconds(actualDuration);
        }
        else
        {
            // Usa el tiempo manual si no hay timeline
            yield return new WaitForSeconds(animationDuration);
        }

        
        buttonSemilla.SetActive(true);
        // Espera adicional antes de activar el cangrejo
        yield return new WaitForSeconds(2f);

        // Activa el cangrejo y limpia
        if (crab != null)
        {
            crab.GetComponent<CrabInteractions>().enabled = true;
        }

        FinalizeProcess();
    }
    private void FinalizeProcess()
    {
        growthCompleted = true;

        if (seedManager != null && mangroves!= null)
        {
            Destroy(mangroves.GetComponent<MangroveInteraction>());
            Destroy(seedManager);
        }

        Destroy(this);
    }
}
