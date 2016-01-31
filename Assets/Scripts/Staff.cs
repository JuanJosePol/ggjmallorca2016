﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Staff : MonoBehaviour
{
    [HideInInspector]
	public Walker walker;
	public float stamina=100;

    [HideInInspector]private IStaffAssignation assignation;
	[HideInInspector]public StaffRenderer staffRenderer;
	GameObject selectionArrow;
	
	static string[] nameArray = {"Sergi Lorenzo", "Juanjo Pol", "Javi Cepa", "Elena Blanes", "Curro Campos", "Alberto Rico", "Alejandro Rico", "David Rico", "Jesús Fernández", "Espe Olea", "Aina Ferriol"};
	static List<string> remainingNames;
	
	void GenerateStaffName() {
		int selection=Random.Range(0,remainingNames.Count);
		name=remainingNames[selection];
		remainingNames.Remove(name);
	}
	
	void Update() {
		if (assignation!=null) {
			stamina-=Time.deltaTime;
		} else {
			stamina+=Time.deltaTime;
		}
		stamina=Mathf.Clamp(stamina, 0, 100);
		if (stamina<=1) {
			Unassign();
		}
	}
	
    void Awake()
    {
	    GameManager.instance.staff.Add(this);
	    remainingNames=new List<string>(nameArray);
	    walker = gameObject.AddComponent<Walker>();
	    staffRenderer=GetComponentInChildren<StaffRenderer>();
	    selectionArrow=GetComponentInChildren<Bounce>().gameObject;
	    selectionArrow.SetActive(false);
    }
	
	public void Select() {
		selectionArrow.SetActive(true);
	}
	
	public void Deselect() {
		selectionArrow.SetActive(false);
	}
	
	void Start() {
		InfoSlotListManager.instanceStaffList.AddStaffSlot(this);
		GenerateStaffName();
	}

    public void Assign(IStaffAssignation newAssignation)
    {
	    if (stamina>20) {
	    	if (assignation != null) {assignation.UnassignStaff();}
		    assignation = newAssignation;
		    transform.DOPunchPosition(Vector3.up*0.5f, 0.5f, 0, 1).SetEase(Ease.OutBounce);
	    } else {
	    	//Try to sleep
	    }
    }

    public void Unassign()
    {
        if (assignation != null)
        {
            assignation.UnassignStaff();
            assignation = null;
        }
    }

    public void OnClick()
    {
        GameManager.instance.SelectStaff(this);
    }
}
