using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Staff : MonoBehaviour
{
    [HideInInspector]
	public Walker walker;
	public float stamina=100;

    [HideInInspector]
    private IStaffAssignation assignation;
	
	public StaffRenderer staffRenderer;
	
	static string[] nameArray = {"Sergi Lorenzo", "Juanjo Pol", "Javi Cepa", "Elena Blanes", "Curro Campos", "Alberto Rico", "Alejandro Rico", "David Rico", "Jesús Fernández", "Espe Olea", "Aina Ferriol"};
	static List<string> remainingNames;
	
	void GenerateStaffName() {
		int selection=Random.Range(0,remainingNames.Count);
		name=remainingNames[selection];
		remainingNames.Remove(name);
	}
	
    void Awake()
    {
	    GameManager.instance.staff.Add(this);
	    remainingNames=new List<string>(nameArray);
	    walker = gameObject.AddComponent<Walker>();
	    staffRenderer=GetComponentInChildren<StaffRenderer>();
    }
	
	void Start() {
		InfoSlotListManager.instanceStaffList.AddStaffSlot(this);
		GenerateStaffName();
	}

    public void Assign(IStaffAssignation newAssignation)
    {
        if (assignation != null) assignation.UnassignStaff();
        assignation = newAssignation;
    }

    public void Unassign()
    {
        if (assignation != null)
        {
            assignation.UnassignStaff();
            assignation = null;
        }
    }
}
