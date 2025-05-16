using System.Collections.Generic;
using UnityEngine;

public class SwitchFishPaths : MonoBehaviour
{
    public FishPathFollowing fishPathFollowing; // Referencia al script FishPathFollowing
    public List<Transform> newWaypoints; // Lista de nuevos waypoints asignados en el inspector
    private List<Transform> oldWaypoints = new List<Transform>(); // Lista de waypoints originales del pez

    public void SwitchPath()
    {
        if (fishPathFollowing == null) {
            Debug.LogError("No se ha asignado una referencia al FishPathFollowing.");
            return;
        }

        // Guardar siempre la lista actual antes de cambiarla
        oldWaypoints.Clear();
        oldWaypoints.AddRange(fishPathFollowing.waypoints);

        // Asignar la nueva lista de waypoints
        fishPathFollowing.waypoints = new List<Transform>(newWaypoints);

        // Reiniciar el movimiento para seguir el nuevo camino
        fishPathFollowing.RestartPatrol();
    }

    public void RestoreOriginalPath()
    {
        if (fishPathFollowing == null) {
            Debug.LogError("No se ha asignado una referencia al FishPathFollowing.");
            return;
        }

        if (oldWaypoints.Count == 0) {
            Debug.LogWarning("No hay waypoints originales almacenados para restaurar.");
            return;
        }

        // Restaurar la lista original de waypoints
        fishPathFollowing.waypoints = new List<Transform>(oldWaypoints);

        // Reiniciar el movimiento para seguir el camino original
        fishPathFollowing.RestartPatrol();
    }

}
