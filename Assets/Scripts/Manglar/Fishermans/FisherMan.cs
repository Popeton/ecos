using EPOOutline;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FisherMan : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject fisherman;
    [SerializeField] private GameObject fish;
    [SerializeField] private VRInteractionHandler interactionHandler;

    //[Header("Componentes")]
    private XRSimpleInteractable fishermanInteractable;
    private Collider fishermanCollider;
    private Outlinable fishermanOutline;

    private bool _hasInteracted = false;

    void Start()
    {
        InitializeComponents();
        RegisterInteraction();
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Sound5);
    }

    private void InitializeComponents()
    {
        // Obtener componentes si no están asignados
        if (fishermanInteractable == null) fishermanInteractable = fisherman.GetComponent<XRSimpleInteractable>();
        if (fishermanCollider == null) fishermanCollider = fisherman.GetComponent<Collider>();
        if (fishermanOutline == null) fishermanOutline = fisherman.GetComponent<Outlinable>();

        // Desactivar inicialmente
        fishermanInteractable.enabled = false;
        fishermanCollider.enabled = false;
        fishermanOutline.enabled = false;
    }

    private void RegisterInteraction()
    {
        if (interactionHandler != null)
        {
            interactionHandler.AddInteractable(fishermanInteractable);
            interactionHandler.OnInteractionStarted += HandleFishermanInteraction;
        }
    }

    void Update()
    {
        float currentTime = AudioManager.instance.GetTimelinePosition() / 1000f;

        if (currentTime >= 10.681f && !_hasInteracted)
        {
            EnableFishermanComponents();
        }
    }

    private void EnableFishermanComponents()
    {
        fishermanInteractable.enabled = true;
        fishermanCollider.enabled = true;
        fishermanOutline.enabled = true;
    }

    private void HandleFishermanInteraction(XRSimpleInteractable interactable)
    {
        if (interactable != fishermanInteractable || _hasInteracted) return;

        _hasInteracted = true;
        ActivateFish();
        DisableFishermanComponents();
    }

    private void DisableFishermanComponents()
    {
        fishermanInteractable.enabled = false;
        fishermanCollider.enabled = false;
        fishermanOutline.enabled = false;
    }

    public void ActivateFish()
    {
        AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion5_Fase1, AudioManager.instance.transform.position);

        if (!fish.activeInHierarchy)
        {
            fish.SetActive(true);
          //  Invoke("CrabDestroy", 5f);
            Debug.Log("Peces activados");
        }
    }

   public void CanoaGone()
    {
        this.GetComponent<Animator>().SetTrigger("CanoaGone");
    }
    

    void OnDestroy()
    {
        if (interactionHandler != null)
        {
            interactionHandler.OnInteractionStarted -= HandleFishermanInteraction;
        }
    }
}