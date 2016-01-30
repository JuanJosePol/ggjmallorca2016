using UnityEngine;
using System.Collections;

public class Bathroom : MonoBehaviour
{
    public float poopTime = 5;
    public Transform enterPosition;
    public Transform poopPosition;
    public WaitingQueue waitingQueue;
    public MeshFilter WCMeshFilter;
    public MeshFilter ClosedWC;
    public MeshFilter OpenWC;


    [HideInInspector]
    public Jammer jammer;

    void Awake()
    {
        GameManager.instance.bathrooms.Add(this);
        WCMeshFilter.mesh = OpenWC.sharedMesh;
    }

    public bool CanEnterJammer()
    {
        return jammer == null || !waitingQueue.isFull;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (!CanEnterJammer()) throw new System.Exception("No more jammers allowen in Bathroom.");

        if (this.jammer == null)
        {
            this.jammer = newJammer;
            jammer.walker.MoveTo(enterPosition.position, true, OnJammerReady);
        }
        else
        {
            waitingQueue.AddJammer(newJammer);
        }
    }

    private void OnJammerReady()
    {
        Process();
    }

    public void Process()
    {
        // Enter the bathroom
        jammer.walker.MoveTo(poopPosition.position, false, () => { StartCoroutine(UseBathroom()); });
    }

    public IEnumerator UseBathroom()
    {
        WCMeshFilter.mesh = ClosedWC.sharedMesh;
        float timeLeft = poopTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        ExitBathroom();
    }

    public void ExitBathroom()
    {
        WCMeshFilter.mesh = OpenWC.sharedMesh;
        jammer.walker.MoveTo(enterPosition.position, false, () => { FreeBathroom(); });
    }

    public void FreeBathroom()
    {
        jammer.assignedTable.AddJammer(jammer);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Jammer>() == jammer)
        {
            jammer = null;
            if (!waitingQueue.isEmpty)
                AddJammer(waitingQueue.GetNextJammer());
        }
    }
}
