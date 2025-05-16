using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject uiPopups;
    [SerializeField] private Transform head;
    [SerializeField] private bool followPlayer = true; // Seguir al jugador
    [SerializeField] private bool onlyOrient = false;  // Solo orientarse hacia el jugador
    [SerializeField] private float spawnDistance = 4f;
    private bool isPositioningActive = true;

    private void Start()
    {
        head = Camera.main.transform;
        PositionUI();
    }

    private void Update()
    {
        if (isPositioningActive)
        {
            if (followPlayer)
            {
                UiFollowPlayer.FollowCamera(uiPopups, head, spawnDistance);
            }
            else if (onlyOrient)
            {
                UiFollowPlayer.OrientTowardsCamera(uiPopups, head);
            }
        }
    }

    private void PositionUI()
    {
        if (followPlayer)
        {
            UiFollowPlayer.FollowCamera(uiPopups, head, spawnDistance);
        }
        else if (onlyOrient)
        {
            UiFollowPlayer.OrientTowardsCamera(uiPopups, head);
        }
        else
        {
            UiFollowPlayer.SetPositionOnce(uiPopups, head, spawnDistance);
        }
    }
}
