using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public GameObject[] chairPosition;

    public List<Jammer> jammers = new List<Jammer>();

    public bool hasRoom { get { return assignedChairs.Count < chairPosition.Length; } }

    [HideInInspector]
    public Dictionary<Jammer, Transform> assignedChairs = new Dictionary<Jammer, Transform>();

    private Game currentGame;

    void Awake()
    {
        GameManager.instance.tables.Add(this);
    }

    void Update()
    {
        // If there are no jammers there is nothing to do
        if (jammers.Count == 0)
            return;

        if (currentGame == null)
        {
            currentGame = GameManager.instance.CreateNewGame();
        }

        currentGame.Develop(jammers.Count);

        if (currentGame.progress >= 1)
            currentGame = null;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (!assignedChairs.ContainsKey(newJammer))
        {
            if (!hasRoom) throw new System.Exception("No more jammer allowed in table");
            assignedChairs.Add(newJammer, chairPosition[assignedChairs.Count].transform);
            newJammer.AssignTable(this);
        }

        newJammer.walker.MoveTo(assignedChairs[newJammer].position,() =>
        {
            newJammer.walker.TurnTo(assignedChairs[newJammer].position);
            StartWorking(newJammer);
        });
    }

    public void StartWorking(Jammer jammer)
    {
        jammers.Add(jammer);
    }

    public void StopWorking(Jammer jammer)
    {
        jammers.Remove(jammer);
    }
}
