using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    [field: SerializeField] public List<EventInstance> pausableInstances; // Lista de eventos pausables (ej. diálogos, efectos interactivos)
    [field: SerializeField] public List<EventInstance> nonPausableInstances; // Lista de eventos no pausables (ej. sonidos ambientales)
    [field: SerializeField] private List<EventInstance> activeOneShots; // Lista de sonidos PlayOneShot

    public static AudioManager instance { get; private set; } // Instancia singleton del AudioManager
    public EventInstance voiceEventInstance; // Instancia del evento de voz
    private bool isManuallyStopped = false; // Controla si el evento fue detenido manualmente

    void Awake()
    {
        if (instance != null)
        {
            UnityEngine.Debug.LogError("Error, más de una instancia en la escena");
        }
        instance = this;

        pausableInstances = new List<EventInstance>();
        nonPausableInstances = new List<EventInstance>();
        activeOneShots = new List<EventInstance>();
    }
    private void Start()
    {
        PlayAmbientSound(FmodEvents.instance.Ambient, transform.position);
    }

    /// <summary>
    /// Reproduce el audio correspondiente a la zona en la que se encuentra el usuario.
    /// </summary>
    public void PlayZoneAudio(string sceneName)
    {
        switch (sceneName)
        {
            case "Home":
                InitializeVoice(FmodEvents.instance.Voice, transform.position);
                break;
            case "Manglar":
                InitializeVoice(FmodEvents.instance.Voice, transform.position);
                
                break;
            default:
                UnityEngine.Debug.LogWarning("No hay evento de voz para esta escena.");
                break;
        }
    }

    

    /// <summary>
    /// Método para buscar sonidos por nombre
    /// </summary>
    public EventReference GetSoundByName(string soundName)
    {
        if (FmodEvents.instance == null)
        {
            Debug.LogError("FmodEvents instance no encontrada!");
            return new EventReference();
        }

        switch (soundName)
        {
            case "Sound1": return FmodEvents.instance.Sound1;
            case "Sound2": return FmodEvents.instance.Sound2;
            case "Sound3": return FmodEvents.instance.Sound3;
            case "Sound4": return FmodEvents.instance.Sound4;
            case "Sound5": return FmodEvents.instance.Sound5;
            case "Sound6": return FmodEvents.instance.Sound6;
            case "Sound7": return FmodEvents.instance.Sound7;
            case "Sound8": return FmodEvents.instance.Sound8;
            case "Sound9": return FmodEvents.instance.Sound9;
            case "Guia": return FmodEvents.instance.Guia;
            case "ButtonOut": return FmodEvents.instance.buttonOut;
            default:
                Debug.LogError($"Sonido {soundName} no encontrado!");
                return new EventReference();
        }
    }

    /// <summary>
    /// Método para UI (se puede usar desde  un OnClick)
    /// </summary>
    public void PlaySoundByName(string soundName)
    {
        EventReference soundEvent = GetSoundByName(soundName);
        PlayOneShot(soundEvent);
    }

    public void PlayOneShot(EventReference sound)
    {
        if (sound.IsNull)
        {
            Debug.LogWarning("Intentando reproducir un sonido vacío.");
            return;
        }

        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        instance.start();
        activeOneShots.Add(instance);
    }

    /// <summary>
    /// Detiene todos los sonidos reproducidos con PlayOneShot.
    /// </summary>
    public void StopAllOneShots()
    {
        foreach (EventInstance instance in activeOneShots)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        activeOneShots.Clear();
    }

    /// <summary>
    /// Crea una nueva instancia de sonido, opcionalmente pausable o no.
    /// </summary>
    public EventInstance CreateInstance(EventReference eventReference, bool isPausable )
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        if (isPausable)
            pausableInstances.Add(eventInstance);
        else
            nonPausableInstances.Add(eventInstance);
        return eventInstance;
    }

    /// <summary>
    /// Modifica un parámetro específico de un evento de sonido.
    /// </summary>
    public void SetParameterByName(EventInstance eventInstance, string parameterName, float value)
    {
        if (eventInstance.isValid())
        {
            eventInstance.setParameterByName(parameterName, value);
        }
        else
        {
            UnityEngine.Debug.LogError("El EventInstance no es válido o no está inicializado correctamente.");
        }
    }

    /// <summary>
    /// Modifica un parámetro global en FMOD.
    /// </summary>
    public void SetGlobalParameter(string parameterName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }

    /// <summary>
    /// Reproduce un segmento de voz desde un punto específico.
    /// </summary>
    public void PlayVoiceSegment(EventReference voiceEvent, float startTime)
    {
        if (voiceEventInstance.isValid())
        {
            voiceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        voiceEventInstance = RuntimeManager.CreateInstance(voiceEvent);
        RuntimeManager.AttachInstanceToGameObject(voiceEventInstance, transform, GetComponent<Rigidbody>());
        voiceEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        voiceEventInstance.setTimelinePosition((int)(startTime * 1000)); // Convertir segundos a milisegundos
        voiceEventInstance.start();
    }

    /// <summary>
    /// Pausa la reproducción de la voz actual.
    /// </summary>
    public void PauseVoice()
    {
        if (voiceEventInstance.isValid())
        {
            isManuallyStopped = true;
            voiceEventInstance.setPaused(true);
        }
    }

    /// <summary>
    /// Reanuda la reproducción de la voz pausada.
    /// </summary>
    public void ResumeVoice()
    {
        if (voiceEventInstance.isValid())
        {
            isManuallyStopped = false;
            voiceEventInstance.setPaused(false);
        }
    }

    /// <summary>
    /// Pausa todos los sonidos pausables.
    /// </summary>
    public void PauseAllAudio()
    {
        foreach (EventInstance eventInstance in pausableInstances)
        {
            eventInstance.setPaused(true);
        }
    }

    /// <summary>
    /// Reanuda todos los sonidos pausables.
    /// </summary>
    public void ResumeAllAudio()
    {
        foreach (EventInstance eventInstance in pausableInstances)
        {
            eventInstance.setPaused(false);
        }
    }

    /// <summary>
    /// Reproduce un sonido ambiental que no puede ser pausado.
    /// </summary>
    public void PlayAmbientSound(EventReference sound, Vector3 position)
    {
        EventInstance instance = CreateInstance(sound, false);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
    }

    /// <summary>
    /// Inicializa un evento de voz y lo reproduce.
    /// </summary>
    public void InitializeVoice(EventReference voiceEventReferent, Vector3 position)
    {
        voiceEventInstance = CreateInstance(voiceEventReferent,true);
        voiceEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        voiceEventInstance.start();
    }

    /// <summary>
    /// Obtiene la duración total del evento de voz en milisegundos.
    /// </summary>
    public int GetTimelineLength()
    {
        if (voiceEventInstance.isValid())
        {
            voiceEventInstance.getDescription(out EventDescription description);
            description.getLength(out int length);
            return length;
        }
        return 0;
    }

    /// <summary>
    /// Obtiene la posición en la línea de tiempo del primer sonido en reproducción dentro de la lista nonPausableInstances.
    /// </summary>
    /// <returns>La posición actual del audio en milisegundos, o -1 si no hay sonidos en reproducción.</returns>
    public int GetTimelinePosition()
    {
        voiceEventInstance.getTimelinePosition(out int timelinePosition);
        return timelinePosition;
    }
    /// <summary>
    /// Detiene y libera la instancia del evento de voz actual.
    /// </summary>
    public void StopAudio()
    {
        voiceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        voiceEventInstance.release();
    }

    /// <summary>
    /// Limpia y libera todos los eventos de audio activos.
    /// </summary>
    public void CleanUp()
    {
        CleanList(pausableInstances);
      //  CleanList(nonPausableInstances);
    }

    /// <summary>
    /// Método auxiliar para limpiar listas de instancias de sonido.
    /// </summary>
    private void CleanList(List<EventInstance> list)
    {
        foreach (EventInstance eventInstance in list)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        list.Clear();
    }

    public void CleanAll()
    {
        CleanList(pausableInstances);
        CleanList(nonPausableInstances);
        CleanList(activeOneShots);
    }
}
