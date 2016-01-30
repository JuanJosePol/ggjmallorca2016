﻿using UnityEngine;
using System.Collections;

public class Jammer : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;

    //private GameObject chair;
    [HideInInspector]
    public Table assignedTable { get; private set; }
<<<<<<< HEAD
	
	static string[] names   ={"Sergi",  "Juanjo", "Justo", "Javi", "Ricardo", "Gabriel", "Pedro", "Yann", "Arnold", "Harry", "Walter", "James", "Luke", "Pepa", "Peter", "Juana", "Lynda", "Robert", "Kylo", "Kate", "Espe", "James", "Judith", "Lucy", "Mary", "Anna", };
	static string[] surnames={"Lorenzo", "Pol", "Cerdá", "Cepa", "Alarcón", "Muntaner", "Zapata", "Olea", "Pope", "White", "Vader", "Skywalker", "Ren", "Carusa", "Cupcake", "Redford", "Icecream", "Rainbow"};
	
	void GenerateJammerName() {
		name=names.GetRandom()+" "+surnames.GetRandom();
	}
	
=======

    public bool isWorking
    {
        get
        {
            return assignedTable != null && assignedTable.jammers.Contains(this);
        }
    }

>>>>>>> 22d4a2bf805e9ce87cacfd6f1e3f5b867ecf6374
    void Awake()
    {
        GameManager.instance.jammers.Add(this);
	    walker = gameObject.AddComponent<Walker>();
	    walker.isJammer=true;
	    GenerateJammerName();
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

#if UNITY_EDITOR
    public void GoToBathroom()
    {
        FindFreeBathroom();
    }
#endif
}
