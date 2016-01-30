﻿using UnityEngine;
using System;
using System.Collections;

public class Walker : MonoBehaviour
{
    private NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
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

    public void TurnTo(Vector3 position)
    {
        StartCoroutine(TurnCoroutine(position));
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

    IEnumerator TurnCoroutine(Vector3 targetPos)
    {
        Vector3 targetDir = (targetPos - transform.position).normalized;
        while ((targetDir - transform.forward).magnitude > 0.1f)
        {
            Vector3 dir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 2, 1);
            transform.rotation = Quaternion.LookRotation(dir);
            yield return null;
        }
    }
}
