using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using FMODUnity;

public class TriggerSoundHandler : MonoBehaviour
{
    [Header("Input Configuration")]
    [SerializeField] private InputActionProperty triggerButtonAction; // Igual que en VRInteractionHandler

    [Header("FMOD Events")]
    [SerializeField] private EventReference triggerPressSound;
    [SerializeField] private EventReference triggerReleaseSound;

    [Header("Settings")]
    [SerializeField] private Transform soundPosition;
    [SerializeField][Range(0.1f, 0.9f)] private float triggerThreshold = 0.2f;

    private bool isTriggerPressed = false;
    private float currentTriggerValue = 0f;

    void Update()
    {
        float previousTriggerValue = currentTriggerValue;
        currentTriggerValue = triggerButtonAction.action.ReadValue<float>();

        // Detección de presión
        if (!isTriggerPressed && currentTriggerValue >= triggerThreshold)
        {
            OnTriggerPressed();
        }
        // Detección de liberación
        else if (isTriggerPressed && currentTriggerValue < triggerThreshold)
        {
            OnTriggerReleased();
        }
    }

    private void OnTriggerPressed()
    {
        isTriggerPressed = true;
        PlaySound(triggerPressSound);
    }

    private void OnTriggerReleased()
    {
        isTriggerPressed = false;
        PlaySound(triggerReleaseSound);
    }

    private void PlaySound(EventReference soundEvent)
    {
        if (!soundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(soundEvent, soundPosition.position);
        }
    }

    private void OnEnable()
    {
        triggerButtonAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerButtonAction.action.Disable();
    }
}