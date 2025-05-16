using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MenuConfiguration : MonoBehaviour
{
    [SerializeField] private AudioManager audioInstance;
   
    [SerializeField] private GameObject uiPopups;
    [SerializeField] private RuneActivationSystem rune;
    [SerializeField] private PathMover GuiapathMover;

    [Header("Cuenta Regresiva")]
    [SerializeField] private CountdownSystem countdownSystem;

    private bool lastAudio = false; 
    // Start is called before the first frame update
    void Start()
    {
        countdownSystem.OnCountdownComplete.AddListener(InitializeExperience);
       
    }
    private void InitializeExperience()
    {
        GuiapathMover.gameObject.SetActive(true);
        rune = this.gameObject.GetComponent<RuneActivationSystem>();
        audioInstance.PlayZoneAudio("Home");
        GuiapathMover.StartExperience();
    }

        // Update is called once per frame
        void Update()
    {
        float currentTime = audioInstance.GetTimelinePosition() / 1000f;

        if (currentTime >= 23.444f )
        {
            uiPopups.SetActive(true);
            audioInstance.StopAudio();
           //
        }

        if (currentTime >= 0.966f && lastAudio )
        {
            print("audiostop");
            // Activa runa y verifica condiciones
           // rune.ActivateRune(0);

            //if (rune.CheckActivationConditions())
            //{
            //    // Acciones al completar todas las runas
            //    NextScene();
            //}

        }
    }

   public void PlayRuneAudio(int runeAudio)
    {
       
        if (!lastAudio)
        {
            switch (runeAudio)
            {
                case 0: audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, audioInstance.transform.position); break;
                case 1: audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase2, audioInstance.transform.position); break;
                case 2: audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase3, audioInstance.transform.position); break;
                case 3: audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase4, audioInstance.transform.position); break;
                case 4: audioInstance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, audioInstance.transform.position); break;
                case 5: Invoke("LastAudio", 7f);  break;
            }
        }
    }

    private void LastAudio()
    {
        audioInstance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, audioInstance.transform.position);
        lastAudio = true;
        StartCoroutine(WaitForAudioCompletion());
    }

    private IEnumerator WaitForAudioCompletion()
    {
        // Esperar hasta que el audio termine
        //while (audioInstance.GetTimelinePosition() < audioInstance.GetTimelineLength() - 500) // Margen de seguridad
        //{
        //    yield return null; // Esperar el siguiente frame
        //}
        rune.LowerNonRequiredRunes();
        // Esperar 10 segundos adicionales antes de activar las runas
        yield return new WaitForSeconds(2f);

        // Llamar a CheckProgress() después de la espera
        rune.EnableRequiredInteractions();
    }


}
