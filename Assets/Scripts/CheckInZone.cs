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

        assignedStaff = newStaff;
        assignedStaff.walker.MoveTo(staffPosition.position, OnStaffReady);
    }

    public void UnassignStaff()
    {
        staffReady = false;
        throw new System.NotImplementedException();
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
        StartCoroutine(Processing());
        // TODO Notify first jammer
        // TODO Remove first jammer from queue
        // if there are more jammers
            // TODO Move jammers
    }

    IEnumerator Processing()
    {
        float timeLeft = CheckInTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
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
        Gizmos.DrawCube(transform.TransformPoint(c.center), c.size);
    }
}
