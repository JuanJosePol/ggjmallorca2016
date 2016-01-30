using UnityEngine;
using System.Collections.Generic;

public class WaitingQueue : MonoBehaviour
{
    public int MaxCapacity;
    public float interspace = 1;

    [HideInInspector]
    public Transform[] positions;

    private List<Jammer> jammers = new List<Jammer>();

    void Awake()
    {
        positions = new Transform[MaxCapacity];

        for (int i = 0; i < MaxCapacity; i++)
        {
            var t = new GameObject("wait"+i).transform;
            t.parent = this.transform;
            t.localPosition = Vector3.zero - Vector3.forward * i * interspace;
            positions[i] = t;
        }
    }

    public void AddJammer(Jammer jammer)
    {
        jammers.Add(jammer);
        jammer.walker.MoveTo(positions[jammers.Count - 1].position);
    }

    public Jammer GetNextJammer()
    {

        Jammer j = jammers[0];
        jammers.RemoveAt(0);
        for (int i = 0; i < jammers.Count; i++)
        {
            jammers[i].walker.MoveTo(positions[i].position);
        }
        return j;
    }

    public bool isFull
    {
        get { return jammers.Count >= MaxCapacity; }
    }

    public bool isEmpty
    {
        get { return jammers.Count == 0; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < MaxCapacity; i++)
        {
	        Gizmos.DrawWireSphere(transform.position - transform.forward * i * interspace, 0.5f);
        }
    }
}
