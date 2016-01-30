using UnityEngine;
using System.Collections;

public class Jammer : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;

    //private GameObject chair;
    [HideInInspector]
    public Table assignedTable { get; private set; }

    void Awake()
    {
        GameManager.instance.jammers.Add(this);
	    walker = gameObject.AddComponent<Walker>();
	    walker.isJammer=true;
    }

    public void AssignTable(Table newTable)
    {
        assignedTable = newTable;
    }

    // This is a Debug Function
    public void FindFreeTable()
    {
        Debug.LogWarning("Debug Function called: Jammer.FindFreeTable");
        foreach (var t in GameManager.instance.tables)
        {
            if (t.hasRoom)
            {
                t.AddJammer(this);
                break;
            }
        }
    }

    public void FindFreeBathroom()
    {
        Debug.LogWarning("Debug Function called: Jammer.FindFreeBathroom");
        foreach (var b in GameManager.instance.bathrooms)
        {
            if (b.CanEnterJammer())
            {
                b.AddJammer(this);
                this.assignedTable.RemoveJammer(this);
            }
        }
    }

#if UNITY_EDITOR
    public void GoToBathroom()
    {
        FindFreeBathroom();
    }
#endif
}
