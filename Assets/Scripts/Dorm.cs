using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Dorm : MonoBehaviour, IStaffAssignation
{
    public List<Transform> beds = new List<Transform>();

    [HideInInspector]
    public List<Staff> staff = new List<Staff>();
    [HideInInspector]
    public Dictionary<Staff, Transform> assignedBeds = new Dictionary<Staff, Transform>();

    public bool hasRoom { get { return staff.Count < beds.Count; } }

    void Awake()
    {
        GameManager.instance.dorm = this;
    }

    private Transform PickRandomBed()
    {
        List<Transform> freeBeds = new List<Transform>();
        foreach (var b in beds)
        {
            if (!assignedBeds.ContainsValue(b))
                freeBeds.Add(b);
        }
        return freeBeds[UnityEngine.Random.Range(0, freeBeds.Count)];
    }

    #region IStaffAssignation Implementation

    public void AssignStaff(Staff newStaff)
    {
        if (!hasRoom)
            return;

        newStaff.Assign(this);
        staff.Add(newStaff);
        Transform freeBed = PickRandomBed();
        newStaff.walker.MoveTo(freeBed.position, true, () => { Rest(newStaff, freeBed); });
    }

    public void UnassignStaff()
    {
    }

    public void OnClick()
    {
        if (GameManager.instance.selectedStaff != null)
        {
            this.AssignStaff(GameManager.instance.selectedStaff);
        }
    }

    public void Process()
    {
        throw new NotImplementedException();
    }

    public void OnStaffReady()
    {
        throw new NotImplementedException();
    }
    
    #endregion

    public void Rest(Staff newStaff, Transform bed)
    {
        //newStaff.walker.TurnTo(bed.position + bed.forward);

        newStaff.sleeping = true;
    }

    public void WakeUp(Staff sleeper)
    {
        sleeper.sleeping = false;
        assignedBeds[sleeper] = null;
        staff.Remove(sleeper);
    }
}
