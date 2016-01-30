using UnityEngine;
using System;
using System.Collections;

public class Walker : MonoBehaviour
{
	private NavMeshAgent navAgent;
	public bool isJammer=false;

    void Awake()
    {
	    navAgent = GetComponent<NavMeshAgent>();
	    if (isJammer) {
	    	navAgent.speed=2+(UnityEngine.Random.value-0.5f)*0.2f;
	    } else {
	    	navAgent.speed=2;
	    }
    }

    public void MoveTo(Vector3 position)
    {
        MoveTo(position, null);
    }

    public void MoveTo(Vector3 position, Action onMoveFinished)
    {
        StopAllCoroutines();
        navAgent.SetDestination(position);
        StartCoroutine(MoveCoroutine(onMoveFinished));
    }

    IEnumerator MoveCoroutine(Action onMoveFinished)
    {
        // HACK let the NavAgent update itself
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        while (navAgent.remainingDistance > 0 && navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            yield return null;
        }

        if (onMoveFinished != null)
            onMoveFinished();
    }
}
