using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LogoInteraction : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private VRInteractionHandler interactionHandler;
    [SerializeField] private Animator logoAnimator;
    [SerializeField] private GameObject guideObject;
    [SerializeField] private float startDelay = 4f;
    [SerializeField] private MangroveInteraction mangrove;

    [Header("Configuración")]
    [SerializeField] private string sceneName;

    private bool hasInteracted = false;
    private PathMover pathMover;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        // Configurar interacción

        XRSimpleInteractable interactable = GetComponentInChildren<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactionHandler.AddInteractable(interactable);
            interactionHandler.OnInteractionStarted += HandleLogoInteraction;
        }
        if (guideObject != null)
        {
            pathMover = guideObject.GetComponent<PathMover>();
            guideObject.SetActive(false);
        }
    }

    private void HandleLogoInteraction(XRSimpleInteractable interactable)
    {
        if (hasInteracted) return;

        // Deshabilitar interacción
        GetComponent<Collider>().enabled = false;
        interactable.enabled = false;

        // Iniciar animación y activar primer manglar
        logoAnimator.SetBool("Active", true);
        ActivateFirstMangrove();

        // Programar inicio de la experiencia
        Invoke("StartGuideExperience", startDelay);
        hasInteracted = true;
    }

    private void ActivateFirstMangrove()
    {
        if (mangrove != null)
        {
            mangrove.enabled = true;
            // Activa directamente el primer manglar
            mangrove.SetManglarState(mangrove.GetManglarByIndex(0), true);
        }
    }

    private void StartGuideExperience()
    {
        if (guideObject != null && pathMover != null)
        {
            guideObject.SetActive(true);
            pathMover.StartExperience();
            AudioManager.instance.PlayZoneAudio(sceneName);

            // Ocultar logo
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Debug.LogError("Referencias de la guía no asignadas!");
        }
    }

    private void OnDestroy()
    {
        interactionHandler.OnInteractionStarted -= HandleLogoInteraction;
    }

    public void StarReverseAnimation()
    {
        logoAnimator.SetTrigger("Reverse");
      
    }
}