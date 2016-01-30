using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public GameObject[] chairPosition;

    public List<Jammer> jammers = new List<Jammer>();

    public bool hasRoom { get { return jammers.Count < 4; } }

    void Awake()
    {
        GameManager.instance.tables.Add(this);
    }

    public void AddJammer(Jammer jammer)
    {
        jammers.Add(jammer);
    }
}
