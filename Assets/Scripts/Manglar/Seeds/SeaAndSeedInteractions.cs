using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections;

public class SeaAndSeedInteractions : MonoBehaviour
{
    [Header("Configuración del Mar")]
    [SerializeField] private Transform seaStartPosition;
    [SerializeField] private Transform seaEndPosition;
    [SerializeField] private float seaRiseDuration = 5f;

    [Header("Configuración de Audio y Semillas")]
    [SerializeField] private AudioManager audioInstance;
    [SerializeField] private SeedGrowthManager seedGrowth;
    [SerializeField] private float voiceActivationTime = 8f;

    private bool hasSeaRisen = false;
    private Coroutine seaRiseCoroutine;
   
    void Update()
    {
        HandleSeaRise();
    }

    private void HandleSeaRise()
    {
        float currentTime = audioInstance.GetTimelinePosition() / 1000f;
        int t = 0;
        if (!hasSeaRisen&& t < 1 && currentTime >= voiceActivationTime)
        {
            StartSeaRise();
            hasSeaRisen = true;
             t = 1;
        }

        if (hasSeaRisen && currentTime >= 14.1f)
        {
            CheckSeedActivation();
            hasSeaRisen = false;
        }
    }

    private void StartSeaRise()
    {
        // Detener corrutina si ya está en ejecución
        if (seaRiseCoroutine != null)
        {
            StopCoroutine(seaRiseCoroutine);
        }

        seaRiseCoroutine = StartCoroutine(RiseSeaSmoothly());
    }

    private IEnumerator RiseSeaSmoothly()
    {
        float elapsedTime = 0f;
        Vector3 startPos = seaStartPosition.position;
        Vector3 endPos = seaEndPosition.position;

        while (elapsedTime < seaRiseDuration)
        {
            transform.position = Vector3.Lerp(
                startPos,
                endPos,
                elapsedTime / seaRiseDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar posición final exacta
        transform.position = endPos;
        Debug.Log("Mar en posición final exacta");
    }

    public void InitialiesVoice()
    {
        audioInstance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, audioInstance.transform.position);
    }

    private void CheckSeedActivation()
    {
        hasSeaRisen = false;
        print("Activando semillas");
       seedGrowth.StartSeeds();
       // audioInstance.PlayOneShot(FmodEvents.instance.Sound4);
    }

}