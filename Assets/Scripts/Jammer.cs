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

    // This is a Debug Function
    public void FindFreeTable()
    {
        Debug.LogWarning("Debug Function called");
        foreach (var t in GameManager.instance.tables)
        {
            if (t.jammers.Count < 4)
            {
                t.AddJammer(this);
                this.chair = t.chairPosition[t.jammers.Count - 1];
                break;
            }
        }

        walker.MoveTo(chair.transform.position);
    }
}
