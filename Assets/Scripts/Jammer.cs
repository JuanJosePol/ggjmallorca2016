using UnityEngine;
using System.Collections;

public class Jammer : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;
    
    [HideInInspector]
    public Table assignedTable { get; private set; }
    public bool isWorking
    {
        get
        {
            return assignedTable != null && assignedTable.jammers.Contains(this);
        }
    }
	
	
	static string[] names   ={"Sergi",  "Juanjo", "Justo", "Javi", "Ricardo", "Gabriel", "Pedro", "Yann", "Arnold", "Harry", "Walter", "James", "Luke", "Pepa", "Peter", "Juana", "Lynda", "Robert", "Kylo", "Kate", "Espe", "James", "Judith", "Lucy", "Mary", "Anna", };
	static string[] surnames={"Lorenzo", "Pol", "Cerdá", "Cepa", "Alarcón", "Muntaner", "Zapata", "Olea", "Pope", "White", "Vader", "Skywalker", "Ren", "Carusa", "Cupcake", "Redford", "Icecream", "Rainbow"};

    private TrollStaff trollStaff;
    private WiFiProblem wifiProblem;

	void GenerateJammerName() {
		name=names.GetRandom()+" "+surnames.GetRandom();
	}
	
    void Awake()
    {
        GameManager.instance.jammers.Add(this);
	    walker = gameObject.AddComponent<Walker>();
	    walker.isJammer=true;
	    GenerateJammerName();

        trollStaff = gameObject.GetComponent<TrollStaff>();
        wifiProblem = gameObject.GetComponent<WiFiProblem>();
    }
	
	public void LoadDialog(DialogType dialogType) {
		GetComponentInChildren<JammerDialog>().LoadDialog(dialogType);
	}

    public void HideDialog()
    {
        GetComponentInChildren<JammerDialog>().HideDialog();
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
                this.assignedTable.StopWorking(this);
            }
        }
    }

    public void GoGetFood()
    {
        Debug.LogWarning("Debug Function called: Jammer.GoGetFood");
        foreach (var f in GameManager.instance.foodZones)
        {
            if (f.CanEnterJammer())
            {
                f.AddJammer(this);
                this.assignedTable.StopWorking(this);
            }
        }
    }

    public void HaveWiFiProblem()
    {
        assignedTable.StopWorking(this);
        walker.TurnTo(transform.position - transform.forward);
        wifiProblem.activated = true;
    }
    
    public void TrollStaff()
    {
        assignedTable.StopWorking(this);
        walker.TurnTo(transform.position - transform.forward);
        trollStaff.activated = true;
    }
#if UNITY_EDITOR
    public void GoToBathroom()
    {
        FindFreeBathroom();
    }
#endif
}
