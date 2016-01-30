using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    public float frequency = 1;
	public float amplitude = 1;
	public float swing=10;
	float phase=0;
	
    private NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponentInParent<NavMeshAgent>();
    }


	// Use this for initialization
	void Start () {
		phase=Random.value*100;
		frequency*=1+(Random.value-0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        //if (navAgent.velocity.magnitude > 0.1f)
        //{
        //    float y = transform.position.y;
        //    y += Mathf.Abs(Mathf.Sin(Time.time));

        Vector3 pos = transform.position;
		pos.y = Mathf.Abs(Mathf.Sin(Time.time * frequency+phase)) * navAgent.velocity.magnitude * amplitude;
		transform.position = pos;
		float tilt=0;
		if (navAgent.velocity.magnitude<0.01f) {
			tilt=0;
		} else {
			tilt=(Mathf.Sin(Time.time * frequency+phase)) * navAgent.velocity.magnitude * swing;
		}
		transform.localRotation=Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, tilt);

            //transform.position = transform.position + (Vector3.up * Mathf.Abs(Mathf.Sin(Time.time))) * navAgent.velocity.magnitude;
        //}
        //else {
        //    transform.position = transform.position;
        //}
	}
}
