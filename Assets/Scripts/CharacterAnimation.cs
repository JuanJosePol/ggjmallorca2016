using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    public float frequency = 1;
    public float amplitude = 1;

    private NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponentInParent<NavMeshAgent>();
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (navAgent.velocity.magnitude > 0.1f)
        //{
        //    float y = transform.position.y;
        //    y += Mathf.Abs(Mathf.Sin(Time.time));

        Vector3 pos = transform.position;
        pos.y = Mathf.Abs(Mathf.Sin(Time.time * frequency)) * navAgent.velocity.magnitude * amplitude;
        transform.position = pos;

            //transform.position = transform.position + (Vector3.up * Mathf.Abs(Mathf.Sin(Time.time))) * navAgent.velocity.magnitude;
        //}
        //else {
        //    transform.position = transform.position;
        //}
	}
}
