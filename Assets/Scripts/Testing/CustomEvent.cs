using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Events")]
public class CustomEvent : ScriptableObject
{
    public Action GEvent; //aca se declara el evento

    public void FireEvent() //esta funcion dispara el evento
    {
        GEvent?.Invoke();
    }
}
