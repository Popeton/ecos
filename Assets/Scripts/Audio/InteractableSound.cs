using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSound : MonoBehaviour
{
    [Header("Configuraci�n de Sonido")]
    [Tooltip("Arrastra el evento FMOD desde el proyecto")]
    public EventReference interactionSound;

    public void PlayInteractionSound()
    {
        if (!interactionSound.IsNull)
        {
            AudioManager.instance.PlayOneShot(interactionSound);
        }
    }
}
