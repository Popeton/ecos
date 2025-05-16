
using UnityEngine;
using System.Collections;
public class TestEvent : MonoBehaviour
{
    public CustomEvent evento;
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(EjecutarEvento());
    }
    IEnumerator EjecutarEvento()
    {
        yield return new WaitForSeconds(5f);
        evento.FireEvent();
    }
}
