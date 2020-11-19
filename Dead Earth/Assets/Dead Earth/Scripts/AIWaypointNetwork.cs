using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode
{
    None,
    Connections,
    Paths,
}

public class AIWaypointNetwork : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] public PathDisplayMode displayMode = PathDisplayMode.Connections;
    
    
    [HideInInspector]
    [SerializeField] public int UIStart, UIEnd = 0;
    
    
    [SerializeField] public List<Transform> wayPoints = null;
}
