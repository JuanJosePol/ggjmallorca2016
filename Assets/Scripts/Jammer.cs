using UnityEngine;
using System.Collections;

public class Jammer : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;
    
    private GameObject chair;

    void Awake()
    {
        GameManager.instance.jammers.Add(this);
        walker = gameObject.AddComponent<Walker>();
    }

    public void AssignChair(GameObject newChair)
    {
        this.chair = newChair;
    }

    // This is a Debug Function
    public void FindFreeTable()
    {
        Debug.LogWarning("Debug Function called");
        foreach (var t in GameManager.instance.tables)
        {
            if (t.hasRoom)
            {
                t.AddJammer(this);
                break;
            }
        }
    }
}
