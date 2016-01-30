using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public GameObject[] chairPosition;

    public List<Jammer> jammers = new List<Jammer>();

    public bool hasRoom { get { return jammers.Count < chairPosition.Length; } }

    void Awake()
    {
        GameManager.instance.tables.Add(this);
    }

    public void AddJammer(Jammer newJammer)
    {
        jammers.Add(newJammer);
        newJammer.AssignChair(chairPosition[jammers.Count - 1]);
        newJammer.walker.MoveTo(chairPosition[jammers.Count - 1].transform.position,() => { newJammer.walker.TurnTo(transform.position); });
    }
}
