using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Initiate
{
    static bool areWeFading = false;

    public static void Fade(string sceneName, Color fadeColor, float fadeDuration)
    {
        if (areWeFading)
        {
            Debug.Log("Ya hay un fade en progreso");
            return;
        }

        areWeFading = true;

        GameObject faderObject = new GameObject("Fader");
        Canvas canvas = faderObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            canvas.worldCamera = mainCam;
            canvas.planeDistance = 0.5f;
        }
        else
        {
            Debug.LogWarning("No se encontró Camera.main, el fade podría no funcionar correctamente en VR");
        }

        CanvasScaler scaler = faderObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        faderObject.AddComponent<GraphicRaycaster>();

        Image image = faderObject.AddComponent<Image>();
        image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f); // Empieza transparente

        Fader fader = faderObject.AddComponent<Fader>();
        fader.Setup(image, fadeColor, fadeDuration, sceneName);
        AudioManager.instance.CleanAll();
    }

    public static void DoneFading()
    {
        areWeFading = false;
    }
}
