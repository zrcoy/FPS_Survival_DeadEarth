using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    [SerializeField] AIWaypointNetwork wayPointNetwork = null;
    [SerializeField] int currentWaypoint = 5;
    [SerializeField] bool hasPath = false;
    [SerializeField] bool hasPathPending = false;
    [SerializeField] bool isPathStale = false;
    [SerializeField] NavMeshPathStatus pathStatus;
    [SerializeField] AnimationCurve curve = new AnimationCurve();

    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        if (wayPointNetwork == null)
        {
            return;
        }
        SetNextDestination(false);
    }




    void Update()
    {
        hasPath = m_navAgent.hasPath;
        hasPathPending = m_navAgent.pathPending;
        isPathStale = m_navAgent.isPathStale;
        pathStatus = m_navAgent.pathStatus;

        if (m_navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }

        if ((m_navAgent.remainingDistance.Equals(0f) && !hasPathPending) || pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetNextDestination(true);
        }
        else if (isPathStale)
        {
            // recalculate the path
            SetNextDestination(false);
        }
    }

    void SetNextDestination(bool increment)
    {
        if (!wayPointNetwork.HasWaypoint())
        {
            print("There's no valid waypoint at all in the network: " + wayPointNetwork.name);
            return;
        }
        int incre = increment ? 1 : 0;
        Transform currentDestination = null;

        int nextWaypoint = currentWaypoint + incre >= wayPointNetwork.wayPoints.Count ? 0 : currentWaypoint + incre;
        currentDestination = wayPointNetwork.wayPoints[nextWaypoint];
        if (currentDestination != null)
        {
            m_navAgent.SetDestination(currentDestination.position);
            currentWaypoint = nextWaypoint;
            return;
        }

        currentWaypoint++;
    }

    IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = m_navAgent.currentOffMeshLinkData;
        Vector3 start = m_navAgent.transform.position;
        Vector3 end = data.endPos + m_navAgent.baseOffset * Vector3.up;
        float time = 0.0f;

        while (time <= duration)
        {
            float t = time / duration;
            m_navAgent.transform.position = Vector3.Lerp(start, end, t) + (curve.Evaluate(t) * Vector3.up);
            time += Time.deltaTime;
            yield return null;
        }
        m_navAgent.CompleteOffMeshLink();
    }

    private NavMeshAgent m_navAgent = null;
}
