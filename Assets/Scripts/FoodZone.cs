using UnityEngine;
using System.Collections;

public class FoodZone : MonoBehaviour
{
    public float eatTime = 3;
    public Transform eatingPosition;
    public WaitingQueue waitingQueue;
    public MeshFilter pizzas;
    public MeshFilter[] foodAnimation;

    [HideInInspector]
    public Jammer jammer;

    public bool CanEnterJammer()
    {
        return !waitingQueue.isFull;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (!CanEnterJammer()) throw new System.Exception("No more jammers allowed in FoodZone");

        if (this.jammer == null)
        {
            this.jammer = newJammer;
            jammer.walker.MoveTo(eatingPosition.position, true, OnJammerReady);
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

    }
}
