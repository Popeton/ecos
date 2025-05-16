using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralEffectSoundController : MonoBehaviour
{
    public GameObject coral;
    public float soundDuration;
    private ParticleSystem sporeEffect;
    private bool isRunning;

    private void Awake()
    {
        sporeEffect = coral.GetComponentInChildren<ParticleSystem>();
    }
    public void StartInteraction()
    {
        if(!isRunning) StartCoroutine(SoundAndEffect());
    }

    IEnumerator SoundAndEffect()
    {
        //Aca va el efecto de sonido del audio
        isRunning = true;
        yield return new WaitForSeconds(soundDuration);
        sporeEffect.Play();
        yield return new WaitForSeconds(2.5f);
        isRunning = false;
    }

    //testeo----------------------------------
    private void OnEnable()
    {
        StartInteraction();
        
    }
}
