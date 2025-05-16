using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;
using System.Collections;

public class MangroveInteraction : MonoBehaviour
{
    [Header("Configuración Principal")]
    [SerializeField] private GameObject[] manglares; // Leñoso, Arbustivo, Denso, Colorado
    [SerializeField] private GameObject[] uiMangles;
    [SerializeField] private GameObject growthSeed;
    [SerializeField] private GameObject seed;
    [SerializeField] private AudioManager audioInstance;
    [SerializeField] private VRInteractionHandler interactionHandler;

    private int currentManglarIndex = 0;
    private bool interactionInProgress = false;

    void Start()
    {
        InitializeInteractables();
        InitializeUI();
        StartCoroutine(StartInteractionSequence());
    }

    void OnDestroy()
    {
        interactionHandler.OnInteractionStarted -= HandleMangroveInteraction;
        audioInstance.CleanUp();
    }

    private void InitializeInteractables()
    {
        foreach (GameObject manglar in manglares)
        {
            var interactable = manglar.GetComponentInChildren<XRSimpleInteractable>();
            if (interactable != null)
            {
                interactionHandler.AddInteractable(interactable);
                SetManglarState(interactable.gameObject, false);
            }
        }
        interactionHandler.OnInteractionStarted += HandleMangroveInteraction;
    }

    private IEnumerator StartInteractionSequence()
    {
        yield return new WaitForSeconds(24f);
        SetManglarState(manglares[0], true);
        audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, audioInstance.transform.position);
    }

    private void HandleMangroveInteraction(XRSimpleInteractable interactable)
    {
        if (interactionInProgress) return;

        interactionInProgress = true;
        ProcessManglarInteraction(interactable.gameObject);
       // AdvanceToNextManglar();
        interactionInProgress = false;
    }

    private void ProcessManglarInteraction(GameObject manglar)
    {
        SetManglarState(manglar, false);
        //PlayPhaseAudio(currentManglarIndex + 1);
        ActivateUI(currentManglarIndex);
    }

    public void ContinueFromUI()
    {
        if (currentManglarIndex < uiMangles.Length)
        {
            uiMangles[currentManglarIndex].SetActive(false); // Oculta la ficha actual
        }

        AdvanceToNextManglar();
        interactionInProgress = false;
    }

    private void AdvanceToNextManglar()
    {
        currentManglarIndex++;

        if (currentManglarIndex < manglares.Length)
        {
            SetManglarState(manglares[currentManglarIndex], true);
        }
        else
        {
            FinalizeInteractionSequence();
        }
    }
    public GameObject GetManglarByIndex(int index)
    {
        if (index >= 0 && index < manglares.Length)
        {
            return manglares[index];
        }
        return null;
    }
    public void PlayPhaseAudio(int phaseIndex)
    {
        switch (phaseIndex)
        {
            case 1:
                audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase2, audioInstance.transform.position);
                break;
            case 2:
                audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase3, audioInstance.transform.position);
                break;
        }
    }

    public void FinalizeInteractionSequence()
    {
        growthSeed.SetActive(true);
        seed.SetActive(true);
        interactionHandler.OnInteractionStarted -= HandleMangroveInteraction;
    }
  

    public void SetManglarState(GameObject manglar, bool active)
    {
        var interactable = manglar.GetComponentInChildren<XRSimpleInteractable>();
        var collider = manglar.GetComponentInChildren<Collider>();
        var outline = manglar.GetComponentInChildren<Outlinable>();

        if (interactable) interactable.enabled = active;
        if (collider) collider.enabled = active;
        if (outline) outline.enabled = active;
    }

    private void InitializeUI()
    {
        foreach (GameObject ui in uiMangles)
        {
            ui.SetActive(false);
        }
    }

    private void ActivateUI(int index)
    {
        if (index >= 0 && index < uiMangles.Length)
        {
            uiMangles[index].SetActive(true);
        }
    }
}