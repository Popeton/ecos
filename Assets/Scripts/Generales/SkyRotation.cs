using System.Collections;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    [Header("Skybox Settings")]
    [SerializeField] private Material sunnySkybox;
    [SerializeField] private Material cloudySkybox;
    [SerializeField] private float rotationSpeed = 0.1f;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 2f;

    private bool isCloudy = false;
    private float currentRotation = 0f;

    void Start()
    {
        // Configurar skybox inicial
        RenderSettings.skybox = sunnySkybox;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);
    }

    void Update()
    {
        // Rotación continua del skybox
        currentRotation += Time.deltaTime * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation % 360);
    }

    public void ToggleSkybox()
    {
        if (!isCloudy)
        {
            TransitionToCloudy();
        }
        else
        {
            TransitionToSunny();
        }
    }

    private void TransitionToCloudy()
    {
        float elapsedTime = 0f;
        Material startMaterial = RenderSettings.skybox;

        //while (elapsedTime < transitionDuration)
        //{
        //    RenderSettings.skybox.Lerp(startMaterial, cloudySkybox, elapsedTime / transitionDuration);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        RenderSettings.skybox = cloudySkybox;
        isCloudy = true;
    }

    private void TransitionToSunny()
    {
        float elapsedTime = 0f;
        Material startMaterial = RenderSettings.skybox;

        //while (elapsedTime < transitionDuration)
        //{
        //    RenderSettings.skybox.Lerp(startMaterial, sunnySkybox, elapsedTime / transitionDuration);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        RenderSettings.skybox = sunnySkybox;
        sunnySkybox.SetFloat("_Exposure", 0.45f);
        isCloudy = false;
    }

    // Método para cambio directo desde eventos UI
    public void SetCloudySkybox(bool cloudy)
    {
        if (cloudy && !isCloudy)
        {
            TransitionToCloudy();
        }
        else if (!cloudy && isCloudy)
        {
            TransitionToSunny();
        }
    }
}