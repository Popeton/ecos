using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using EPOOutline;

public class TrashCollectionSystem : MonoBehaviour
{
    [Header("Configuración Basura")]
    [SerializeField] private GameObject[] trashs;
    [SerializeField] private GameObject[] decorativeTrashs; // Nueva basura decorativa
    [SerializeField] private GameObject trahsManager;

    [Header("Configuración Peces")]
    [SerializeField] private GameObject fishManager;
    [SerializeField] private GameObject[] fishObjects;

    [Header("Configuracion Luces")]
    [SerializeField] private Light luzSolar;
    [SerializeField] private GameObject[] psLuzSolar;
    [SerializeField] private SkyRotation skyController;
    [SerializeField] private GameObject[] psLluvias;

    [Header("Configuración Vr")]
    [SerializeField] private VRInteractionHandler interactionHandler;
    [SerializeField] private GameObject guia;

    [Header("Configuración Audio")]
    [SerializeField] private AudioManager audioManager;

    [Header("UI Contador")]

    [SerializeField] private GameObject trashCounterCanvas;
    [SerializeField] private GameObject trashInfoCanvas;
    [SerializeField] private TMP_Text trashCounterText;
    [SerializeField] private TMP_Text totalTrashText;
    private int totalTrash;
    private int collectedTrash;
    private XRSimpleInteractable trahsInteractable;
    private bool hasActivatedInitialTrash;

    private void Start()
    {
        //audioManager.PlayOneShot(FmodEvents.instance.Sound6);
        totalTrash = trashs.Length;
        totalTrashText.text = $"{totalTrash}";
        trahsManager.SetActive(false);
        //totalTrashText.text = totalTrash.ToString();
        collectedTrash = 0;
        UpdateCounterUI();
        foreach (var fish in fishObjects) fish.SetActive(false);
        // Inicializar estado de la basura
        foreach (var trash in trashs)
        {
            trahsInteractable = trash.GetComponent<XRSimpleInteractable>();
            if (trahsInteractable != null)
            {
                interactionHandler.AddInteractable(trahsInteractable);
                trash.GetComponent<Collider>().enabled = false;
                trash.GetComponent<XRSimpleInteractable>().enabled = false;
                trash.GetComponent<Outlinable>().enabled = false;
                trahsInteractable.enabled = false;
            }
        }

        //  basura decorativa al inicio
        foreach (var decoTrash in decorativeTrashs) decoTrash.SetActive(true);
        
        trashInfoCanvas.SetActive(true);
        trashCounterCanvas.SetActive(false);
        interactionHandler.OnInteractionStarted += HandleTrashInteraction;
    }

    private void Update()
    {
        //float currentTime = audioManager.GetTimelinePosition() / 1000f;
        //if (currentTime >= 10f && !hasActivatedInitialTrash)
        //{
            
         
          
        //}
    }

    public void StartTrashGame()
    {
        //  foreach (var decoTrash in decorativeTrashs) decoTrash.SetActive(true);
      // StartCoroutine(ActivateMangarer());
      //foreach (var Trash in trashs) Trash.SetActive(true);
        StartCoroutine(CheckDecorativeActivation());
        trashInfoCanvas.SetActive(false);
        trahsManager.SetActive(true);
        trashs[0].GetComponent<Collider>().enabled = true;
        trashs[0].GetComponent<XRSimpleInteractable>().enabled = true;
        trashs[0].GetComponent<Outlinable>().enabled = true;
        // Activar basura decorativa


        trashCounterCanvas.SetActive(true);

        hasActivatedInitialTrash = true;

    }
    private void HandleTrashInteraction(XRSimpleInteractable interactable)
    {
        if (collectedTrash < totalTrash)
        {
            trashs[collectedTrash].SetActive(false);
            collectedTrash++;
            UpdateCounterUI();

            if (collectedTrash < totalTrash)
            {
                trashs[collectedTrash].GetComponent<Collider>().enabled = true;
                trashs[collectedTrash].GetComponent<XRSimpleInteractable>().enabled = true;
                trashs[collectedTrash].GetComponent<Outlinable>().enabled = true;
            }

            if (collectedTrash >= totalTrash)
            {
                OnAllTrashCollected();
            }
        }
    }

    private IEnumerator CheckDecorativeActivation()
    {
        
        while (!AllDecorativeTrashActive())
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Ahora activar el manager
        
    }

    private bool AllDecorativeTrashActive()
    {
        foreach (var decoTrash in decorativeTrashs)
        {
            if (!decoTrash.activeInHierarchy) return false;
        }
        return true;
    }
    private void UpdateCounterUI()
    {
        trashCounterCanvas.SetActive(true);
        trashCounterText.text = $"{collectedTrash}";
    }

    private IEnumerator  DestroyTrash()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
    private IEnumerator ActivateMangarer()
    {
        yield return new WaitForSeconds(2f);
        trahsManager.SetActive(true);
    }
    private void OnAllTrashCollected()
    {
        audioManager.PlayOneShot(FmodEvents.instance.buttonReward);
        // Desactivar elementos
        foreach (var ps in psLluvias) ps.GetComponent<ParticleSystem>().Stop();
        foreach (var decoTrash in decorativeTrashs) decoTrash.SetActive(false);
        foreach (var Trash in trashs) Trash.SetActive(false);
        trashCounterCanvas.SetActive(false);
        audioManager.StopAllOneShots();
        // Activar efectos finales
        guia.GetComponent<PathMover>().GoToNextPath();
        skyController.SetCloudySkybox(false);
        audioManager.InitializeVoice(FmodEvents.instance.Interaccion7_Fase1, audioManager.transform.position);
        fishManager.SetActive(true);
        luzSolar.intensity = 2.3f;
        foreach (var ps in psLuzSolar) ps.SetActive(true);
        foreach (var fish in fishObjects) fish.SetActive(true);
        Destroy(trahsManager);

        StartCoroutine(DestroyTrash());
    }

    void OnDestroy()
    {
        if (interactionHandler != null)
        {
            Destroy(this.gameObject);
            interactionHandler.OnInteractionStarted -= HandleTrashInteraction;
        }
    }
}