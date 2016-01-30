using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public GameObject[] chairPosition;

    public List<Jammer> jammers = new List<Jammer>();

    public bool hasRoom { get { return jammers.Count < chairPosition.Length; } }

    [HideInInspector]
    public Dictionary<Jammer, Transform> assignedChairs = new Dictionary<Jammer, Transform>();

    void Awake()
    {
        GameManager.instance.tables.Add(this);
    }

    public void AddJammer(Jammer newJammer)
    {

        if (!jammers.Contains(newJammer))
        {
            if (!hasRoom) throw new System.Exception("No more jammer allowed in table");
            jammers.Add(newJammer);
            assignedChairs.Add(newJammer, chairPosition[jammers.Count - 1].transform);
            newJammer.AssignTable(this);
        }

        newJammer.walker.MoveTo(assignedChairs[newJammer].position,() => { newJammer.walker.TurnTo(assignedChairs[newJammer].position); });
    }

    public void RemoveJammer(Jammer oldJammer)
    {
        Debug.Log("Jammer moved away from table");
    }
}
