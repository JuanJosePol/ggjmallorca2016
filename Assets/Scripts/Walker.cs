using UnityEngine;
using System;
using System.Collections;

public class Walker : MonoBehaviour
{
	private NavMeshAgent navAgent;
	public bool isJammer=false;
	
	private float turningSpeed=5;

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
        
        StartCoroutine(MoveCoroutine(position, onMoveFinished));
    }

    public void TurnTo(Vector3 position)
    {
        StopAllCoroutines();
        StartCoroutine(TurnCoroutine(position));
    }

    IEnumerator MoveCoroutine(Vector3 position, Action onMoveFinished)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f));

        navAgent.SetDestination(position);

        float originalRadius = navAgent.radius;
        navAgent.radius = 0.1f;
        // HACK let the NavAgent update itself
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        while (navAgent.remainingDistance > 0 && navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            yield return null;
        }

        navAgent.radius = originalRadius;
        if (onMoveFinished != null)
            onMoveFinished();
    }

    IEnumerator TurnCoroutine(Vector3 targetPos)
    {
        Vector3 targetDir = (targetPos - transform.position).normalized;
        while ((targetDir - transform.forward).magnitude > 0.1f)
        {
	        Vector3 dir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * turningSpeed, 1);
            transform.rotation = Quaternion.LookRotation(dir);
            yield return null;
        }
    }
}
