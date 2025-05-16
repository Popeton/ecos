using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WebInCoralController : MonoBehaviour
{
    [SerializeField] MoveObjectDown webPosRef;
    [SerializeField] FadeOut webMaterial;

    private XRSimpleInteractable webInteractable;
    private bool onInteraction;


    private void Awake()
    {
        webInteractable = GetComponent<XRSimpleInteractable>();
    }
    public void StartWebInteraction(XRSimpleInteractable interactable)
    {
        if(interactable==webInteractable && !onInteraction) {
            StartCoroutine(WebInteraction());
        }
    }

    IEnumerator WebInteraction()
    {
        onInteraction = true;
        webPosRef.MoveDown(); //poner valores negativos para que vaya hacia arriba
        yield return new WaitForSeconds(1f);
        webMaterial.StartFadeOut();
    }
}
