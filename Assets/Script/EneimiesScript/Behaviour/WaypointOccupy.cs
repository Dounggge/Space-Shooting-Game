using System.Collections.Generic;
using UnityEngine;

public static class WaypointOccupancy
{
    private static readonly HashSet<Transform> occupied = new HashSet<Transform>();

    public static bool TryOccupy(Transform waypoint)
    {
        if (waypoint == null) return false;
        return occupied.Add(waypoint);
    }

    public static void Release(Transform waypoint)
    {
        if (waypoint != null) occupied.Remove(waypoint);
    }

    public static bool IsOccupied(Transform waypoint)
    {
        return waypoint != null && occupied.Contains(waypoint);
    }

    public static void Clear() => occupied.Clear();
}