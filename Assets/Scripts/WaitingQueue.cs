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
            t.localPosition = Vector3.zero - (Vector3.forward * i * interspace);
            positions[i] = t;
        }
    }

    public void AddJammer(Jammer newJammer)
    {
        jammers.Add(newJammer);
        Vector3 waitPos = positions[jammers.Count - 1].position;
        waitPos += positions[jammers.Count - 1].right * Random.Range(-0.5f, 0.5f);
        newJammer.walker.MoveTo(waitPos, true,() => { newJammer.walker.TurnTo(this.transform.position + this.transform.forward); });
    }

    public Jammer GetNextJammer()
    {
        Jammer j = jammers[0];
        jammers.RemoveAt(0);
        for (int i = 0; i < jammers.Count; i++)
        {
            jammers[i].walker.MoveTo(positions[i].position, true);
        }
        return j;
    }

    void MoveGuy(int i) {

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
