using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrowCoralController : MonoBehaviour
{
    [SerializeField] SwitchObjects switchRef;
    [SerializeField] MoveObjectDown seed;
    [SerializeField] GameObject coral;
    

    private XRSimpleInteractable coralInteractable;
    void Start()
    {
        this.coralInteractable = GetComponent<XRSimpleInteractable>();
        coral.SetActive(false);
    }

    public void StartCoral(XRSimpleInteractable interactable)
    {

        if (interactable == coralInteractable ) {
            StartCoroutine(StartInteraction());
        }
    }

    IEnumerator StartInteraction()
    {
        switchRef.StartSwitch();
        seed.MoveDown();
        yield return new WaitForSeconds(2.1f);
    }
}
