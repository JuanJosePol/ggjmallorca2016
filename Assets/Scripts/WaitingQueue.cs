using UnityEngine;

public class WaitingQueue : MonoBehaviour
{
    public int MaxCapacity;
    public float interspace = 1;

    [HideInInspector]
    public Transform[] positions;

    void Awake()
    {
        positions = new Transform[MaxCapacity];

        for (int i = 0; i < MaxCapacity; i++)
        {
            var t = new GameObject().transform;
            t.parent = this.transform;
            t.localPosition = Vector3.zero - Vector3.forward * i * interspace;
            positions[i] = t;
        }
    }

    public void AddJammer(Jammer jammer)
    {

    }

    public void GetNextJammer(Jammer jammer)
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform t in positions)
        {
            Gizmos.DrawSphere(t.position, 0.5f);
        }
    }
}
