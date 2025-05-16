using DG.Tweening;
using EPOOutline;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FishManager : MonoBehaviour
{
    [SerializeField] private VRInteractionHandler interactionHandler; // Referencia al VRInteractionHandler
    [SerializeField] private GameObject[] fishUIs; // Ventanas de información para cada pez
    [SerializeField] private GameObject[] fishObjects; // Array de peces con los que se puede interactuar
   
    [SerializeField] private GameObject trashs; // Basura que aparecerá
    [SerializeField] private int maxInteractions = 2; // Número máximo de interacciones permitidas por pez
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject[] PsLluvias;
    [SerializeField] private GameObject[] psLuzSolar;
    [SerializeField] private Light luzSolar;


    private int[] fishInteractionCounts; // Contador por pez para rastrear interacciones
    private bool fishInteractionEnd = false;
    bool timeComplete = false, trashActivated=false;
    int totalInteractions = 0;
    [SerializeField] SkyRotation skyController;
    void Start()
    {
       
        trashs.SetActive(false);
        // Inicializar contadores y desactivar todas las UIs al inicio
        fishInteractionCounts = new int[fishObjects.Length];
        foreach (var fishUI in fishUIs)
        {
            fishUI.SetActive(false);
        }

        // Iterar sobre cada pez y agregarlo al VRInteractionHandler

        // audioManager.AddEndOfEventCallback(audioManager.voiceEventInstance, OnAudioEnd);

        StartCoroutine(ActivateFish(0));
    }
    //private void Update()
    //{
    //    float currentTime = audioManager.GetTimelinePosition() / 1000f;

    //    // Calcular el total de interacciones

    //    totalInteractions = fishInteractionCounts[0] + fishInteractionCounts[1] + fishInteractionCounts[2];

    //    if (currentTime >= 36.096f) timeComplete = true;

    //    if ( timeComplete &&totalInteractions >= 3&&!trashActivated)
    //    {
    //        Invoke("ActivateTrash",2f);
    //        trashActivated = true;

    //    }
    //}
    //private void HandleInteraction(XRSimpleInteractable interactable)
    //{
    //    // Obtener el índice del pez interactuado
    //    int fishIndex = GetFishIndex(interactable);
    //    if (fishIndex == -1) return; // No es un pez válido
        
    //    if (fishInteractionCounts[fishIndex] >= maxInteractions)
    //    {
    //        DisableFishInteraction(fishObjects[fishIndex]);
    //        fishInteractionEnd = true;
    //    }
    //    // Verificar si ya alcanzó el límite de interacciones
    //    if (fishInteractionCounts[fishIndex] >= maxInteractions) return;

    //    // Incrementar el contador de interacciones del pez
    //    fishInteractionCounts[fishIndex]++;

    //    // Mostrar la UI asociada al pez
    //    fishUIs[fishIndex].SetActive(true);

    //    // Deshabilitar la interacción con el pez después de que alcance el máximo permitido
        

    //    // Ocultar la UI después de un tiempo
    //   // Invoke(nameof(HideInfoUI), 10f);
    //}

    private int GetFishIndex(XRSimpleInteractable interactable)
    {
        // Obtener el índice del pez interactuado
        for (int i = 0; i < fishObjects.Length; i++)
        {
            if (fishObjects[i].GetComponent<XRSimpleInteractable>() == interactable)
            {
                return i;
            }
        }
        return -1;
    }

    public void HideInfoUI()
    {
        foreach (var fishUI in fishUIs)
        {
            fishUI.SetActive(false);
        }
    }

    public void ActivatedAudio()
    {
        audioManager.ResumeAllAudio();
    }


    public void PauseAudio()
    {
        audioManager.PauseAllAudio();
    }
    public void ActivateTrash()
    {
       
        luzSolar.intensity = 0.2f;
        foreach (var PsLuz in psLuzSolar) { PsLuz.SetActive(false); }
        foreach (var PsLluvia in PsLluvias) { PsLluvia.SetActive(true); }
        skyController.SetCloudySkybox(true);
       
         Invoke("Rain", 0.1f);
        
        audioManager.InitializeVoice(FmodEvents.instance.Interaccion6_Fase1, audioManager.transform.position);
        
       // Invoke("Chainsaw", 3f);
        
        trashs.SetActive(true);
    
        Invoke(nameof(TurnOffFishManager), 2f);
       
    }

    private void Chainsaw()
    {
        audioManager.PlayOneShot(FmodEvents.instance.Sound8);
    }

    private void Rain()
    {
        audioManager.PlayOneShot(FmodEvents.instance.Sound6);
    }
    public void SoundsActivation(int index)
    {
        switch (index)
        {
            case 1: audioManager.InitializeVoice(FmodEvents.instance.Interaccion5_Fase2, audioManager.transform.position); StartCoroutine(ActivateFish(1)); break;
            case 2: audioManager.InitializeVoice(FmodEvents.instance.Interaccion5_Fase3, audioManager.transform.position); StartCoroutine(ActivateFish(2)); break;
            //case 2: audioManager.InitializeVoice(FmodEvents.instance Interaccion5_Fase3, this.transform.position); StartCoroutine(ActivateFish(2)); break;
            case 3: audioManager.InitializeVoice(FmodEvents.instance.Interaccion5_Fase4, audioManager.transform.position); StartCoroutine(ActivatedTrash()); break;
        }
    }
   
  
    void TurnOffFishManager()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
    private IEnumerator ActivatedTrash()
    {
        yield return new WaitForSeconds(14f);
        //Invoke("Rain", 0.1f);
        ActivateTrash();
        yield return new WaitForSeconds(5f);
        Destroy(this);
    }

   private IEnumerator ActivateFish(int order)
    {
        DisableFishInteraction(fishObjects[order]);
        yield return new WaitForSeconds(8f);
        ActivedFishInteraction(fishObjects[order]);
    }
    private void DisableFishInteraction(GameObject fish)
    {
        XRSimpleInteractable fishInteractable = fish.GetComponent<XRSimpleInteractable>();
        if (fishInteractable != null)
        {
            fishInteractable.enabled = false;
            fish.GetComponent<Outlinable>().enabled = false;
            
            fish.GetComponent<Collider>().enabled = false;
            interactionHandler.RemoveInteractable(fishInteractable);
        }
    }

    private void ActivedFishInteraction(GameObject fish)
    {
        XRSimpleInteractable fishInteractable = fish.GetComponent<XRSimpleInteractable>();
        if (fishInteractable != null)
        {
            fishInteractable.enabled = true;
            //fish.GetComponent<PathMover>().StartExperience();
           
            fish.GetComponent<Outlinable>().enabled = true;
            fish.GetComponent<Collider>().enabled = true;
            interactionHandler.AddInteractable(fishInteractable);
        }
    }
    private void OnDestroy()
    {
        // Limpiar la suscripción al destruir el objeto
       // interactionHandler.OnInteractionStarted -= HandleInteraction;
    }
}
