using UnityEngine;

public class UiFollowPlayer
{
    // Sigue al jugador en todo momento
    public static void FollowCamera(GameObject uiCanvas, Transform cameraPosition, float spawnDistance)
    {
        uiCanvas.transform.position = cameraPosition.position + new Vector3(cameraPosition.forward.x, 0, cameraPosition.forward.z).normalized * spawnDistance;
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, cameraPosition.position.y, cameraPosition.position.z));
    }

    // Posiciona el UI una vez en la dirección actual del jugador y lo deja fijo
    public static void SetPositionOnce(GameObject uiCanvas, Transform cameraPosition, float spawnDistance)
    {
        uiCanvas.transform.position = cameraPosition.position + new Vector3(cameraPosition.forward.x, 0, cameraPosition.forward.z).normalized * spawnDistance;
        FollowPlayerLooK(uiCanvas, cameraPosition);
    }

    // Hace que el UI mire al jugador, sin ajustar su posición
    public static void FollowPlayerLooK(GameObject uiCanvas, Transform cameraPosition)
    {
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, uiCanvas.transform.position.y, cameraPosition.position.z));
    }
    public static void OrientTowardsCamera(GameObject uiCanvas, Transform cameraPosition)
    {
        // Orienta el objeto uiCanvas para que mire hacia la posición de la cámara (cabeza del jugador)
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, uiCanvas.transform.position.y, cameraPosition.position.z));
    }


}
