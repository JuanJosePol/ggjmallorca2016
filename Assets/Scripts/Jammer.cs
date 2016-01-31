using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        trollStaff = gameObject.GetComponentInChildren<TrollStaff>();
        wifiProblem = gameObject.GetComponentInChildren<WiFiProblem>();
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
    
    public void FindFreeTable()
    {
        List<Table> freeTables = new List<Table>();
        foreach (var t in GameManager.instance.tables)
        {
            if (t.hasRoom)
                freeTables.Add(t);
        }

        Table target = freeTables[Random.Range(0, freeTables.Count)];
        target.AddJammer(this);
    }

    public void FindFreeBathroom()
    {
        List<Bathroom> freeBathrooms = new List<Bathroom>();
        foreach (var b in GameManager.instance.bathrooms)
        {
            if (b.CanEnterJammer())
                freeBathrooms.Add(b);
        }

        if (freeBathrooms.Count == 0) return;

        Bathroom target = freeBathrooms[Random.Range(0, freeBathrooms.Count)];
        target.AddJammer(this);
        this.assignedTable.StopWorking(this);
    }

    public void GoGetFood()
    {
        List<FoodZone> freeFoodZones = new List<FoodZone>();
        foreach (var f in GameManager.instance.foodZones)
        {
            if (f.CanEnterJammer())
                freeFoodZones.Add(f);
        }

        if (freeFoodZones.Count == 0) return;

        FoodZone target = freeFoodZones[Random.Range(0, freeFoodZones.Count)];
        target.AddJammer(this);
        this.assignedTable.StopWorking(this);
    }

    public void HaveWiFiProblem()
    {
        assignedTable.StopWorking(this);
        walker.TurnTo(transform.position - transform.forward);
	    wifiProblem.activated = true;
	    if (Random.value>0.5f) {
		    AssetCatalog.instance.PlaySound("help");
	    } else {
		    AssetCatalog.instance.PlaySound("wifi");
	    }
    }
    
    public void TrollStaff()
    {
        assignedTable.StopWorking(this);
        walker.TurnTo(transform.position - transform.forward);
	    trollStaff.activated = true;
	    AssetCatalog.instance.PlaySound("troll");
    }
#if UNITY_EDITOR
    public void GoToBathroom()
    {
        FindFreeBathroom();
    }
#endif
}
