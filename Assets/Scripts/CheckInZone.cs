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
            jammer.walker.MoveTo(checkInPosition.position, true, OnJammerReady);
        }
        else
        {
            waitingQueue.AddJammer(newJammer);
        }
    }

    public void AssignStaff(Staff newStaff)
    {
        if (this.assignedStaff != null)
            return;

        newStaff.Assign(this);
	    AssetCatalog.instance.PlaySound("task");
	    assignedStaff = newStaff;
        assignedStaff.walker.MoveTo(staffPosition.position, false, OnStaffReady);
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
        jammer.LoadDialog(DialogType.Ticket);
        jammerReady = true;
        if (staffReady) Process();

    }
    
    public void Process()
	{
		AssetCatalog.instance.PlaySound("ticketcash");
        StartCoroutine(ProcessCoroutine());
    }

    IEnumerator ProcessCoroutine()
    {
        float timeElapsed = 0;
        while (timeElapsed < CheckInTime)
        {
            timeElapsed += Time.deltaTime;
            assignedStaff.assignmentProgress = timeElapsed / CheckInTime;
            yield return null;
        }
        assignedStaff.assignmentProgress = -1;

        jammerReady = false;
        jammer.HideDialog();
        jammer.FindFreeTable();
        jammer = null;
        if (!waitingQueue.isEmpty)
        {
            AddJammer(waitingQueue.GetNextJammer());
        }
	    
    }

    public void OnClick()
    {
        if (GameManager.instance.selectedStaff != null)
        {
            this.AssignStaff(GameManager.instance.selectedStaff);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        BoxCollider c = GetComponent<BoxCollider>();
	    Gizmos.DrawWireCube(transform.TransformPoint(c.center), c.size);
    }
}
