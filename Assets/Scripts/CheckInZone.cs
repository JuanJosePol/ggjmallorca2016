using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CheckInZone : MonoBehaviour, IStaffAssignation
{
    public float CheckInTime = 2;
    public Transform staffPosition;
    public Transform checkInPosition;
    public WaitingQueue waitingQueue;
    
    [HideInInspector]
    public Staff assignedStaff;
    [HideInInspector]
    public Jammer jammer;

    private bool staffReady = false;
    private bool jammerReady = false;

    public bool CanEnterJammer()
    {
        return GameManager.instance.hasRoomForJammers && !waitingQueue.isFull;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (this.jammer == null)
        {
            this.jammer = newJammer;
            jammer.walker.MoveTo(checkInPosition.position, OnJammerReady);
        }
        else
        {
            waitingQueue.AddJammer(newJammer);
        }
    }

    public void AssignStaff(Staff newStaff)
    {
        if (this.assignedStaff != null) throw new System.Exception("There is staff in this zone already");

        newStaff.Assign(this);
        assignedStaff = newStaff;
        assignedStaff.walker.MoveTo(staffPosition.position, OnStaffReady);
    }

    public void UnassignStaff()
    {
        StopAllCoroutines();
        staffReady = false;
        assignedStaff = null;
    }

    public void OnStaffReady()
    {
        assignedStaff.walker.TurnTo(checkInPosition.position);
        staffReady = true;
        if (jammerReady) Process();
    }

    public void OnJammerReady()
    {
        jammer.walker.TurnTo(staffPosition.position);
        jammerReady = true;
        if (staffReady) Process();

    }
    
    public void Process()
    {
        StartCoroutine(ProcessCoroutine());
    }

    IEnumerator ProcessCoroutine()
    {
        yield return new WaitForSeconds(CheckInTime);
        jammerReady = false;
        jammer.FindFreeTable();
        jammer = null;
        if (!waitingQueue.isEmpty)
        {
            AddJammer(waitingQueue.GetNextJammer());
        }
    }

    public void OnClick()
    {
        Staff staff = FindObjectOfType<Staff>();

        this.AssignStaff(staff);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        BoxCollider c = (BoxCollider)GetComponent<BoxCollider>();
	    Gizmos.DrawWireCube(transform.TransformPoint(c.center), c.size);
    }
}
