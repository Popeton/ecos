using UnityEngine;

public class UiFollowPlayer
{
    // Sigue al jugador en todo momento
    public static void FollowCamera(GameObject uiCanvas, Transform cameraPosition, float spawnDistance)
    {
        uiCanvas.transform.position = cameraPosition.position + new Vector3(cameraPosition.forward.x, 0, cameraPosition.forward.z).normalized * spawnDistance;
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, cameraPosition.position.y, cameraPosition.position.z));
    }

    // Posiciona el UI una vez en la direcci�n actual del jugador y lo deja fijo
    public static void SetPositionOnce(GameObject uiCanvas, Transform cameraPosition, float spawnDistance)
    {
        uiCanvas.transform.position = cameraPosition.position + new Vector3(cameraPosition.forward.x, 0, cameraPosition.forward.z).normalized * spawnDistance;
        FollowPlayerLooK(uiCanvas, cameraPosition);
    }

    // Hace que el UI mire al jugador, sin ajustar su posici�n
    public static void FollowPlayerLooK(GameObject uiCanvas, Transform cameraPosition)
    {
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, uiCanvas.transform.position.y, cameraPosition.position.z));
    }
    public static void OrientTowardsCamera(GameObject uiCanvas, Transform cameraPosition)
    {
        // Orienta el objeto uiCanvas para que mire hacia la posici�n de la c�mara (cabeza del jugador)
        uiCanvas.transform.LookAt(new Vector3(cameraPosition.position.x, uiCanvas.transform.position.y, cameraPosition.position.z));
    }


}
