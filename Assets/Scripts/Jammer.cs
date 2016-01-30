using UnityEngine;
using System.Collections;

public class Jammer : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private GameObject chair;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        GameManager.instance.jammers.Add(this);
    }

	// Use this for initialization
	void Start () {
        FindFreeTable();
	}
	
    public void FindFreeTable()
    {
        foreach (var t in GameManager.instance.tables)
        {
            if (t.jammers.Count < 4)
            {
                t.AddJammer(this);
                this.chair = t.chairPosition[t.jammers.Count - 1];
                break;
            }
        }

        navAgent.SetDestination(chair.transform.position);
    }
}
