using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        network.displayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.displayMode);
        if (network.displayMode == PathDisplayMode.Paths)
        {
            network.UIStart = EditorGUILayout.IntSlider("Start Waypoint", network.UIStart, 0, network.wayPoints.Count - 1);
            network.UIEnd = EditorGUILayout.IntSlider("End Waypoint", network.UIEnd, 0, network.wayPoints.Count - 1);
        }
        // draw the rest part of target as default inspector settings
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        // target : The object being inspected
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        //labels
        for (int i = 0; i < network.wayPoints.Count; i++)
        {
            if (network.wayPoints[i] == null)
            {
                continue;
            }
            Handles.Label(network.wayPoints[i].position, "Waypoint " + i.ToString());
        }

        if (network.displayMode == PathDisplayMode.Connections)
        {
            //connecting poly lines
            Vector3[] linePoints = new Vector3[network.wayPoints.Count + 1];

            for (int i = 0; i <= network.wayPoints.Count; i++)
            {
                int index = i != network.wayPoints.Count ? i : 0;
                if (network.wayPoints[index] == null)
                {
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
                }
                else
                {
                    linePoints[i] = network.wayPoints[index].position;
                }
            }

            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePoints);
        }
        else if (network.displayMode == PathDisplayMode.Paths)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 start = network.wayPoints[network.UIStart].position;
            Vector3 end = network.wayPoints[network.UIEnd].position;
            NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);

            Handles.color = Color.yellow;
            Handles.DrawPolyLine(path.corners);
        }

    }
}
