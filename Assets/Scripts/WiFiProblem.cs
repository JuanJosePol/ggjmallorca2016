using UnityEngine;
using System;
using System.Collections;

public class WiFiProblem : MonoBehaviour, IStaffAssignation
{
    private Vector3 staffPosition { get { return transform.position + transform.forward; } }

    [HideInInspector]
    public Staff assignedStaff;

    private Jammer jammer;
    private bool activated = false;
    private float repairTime = 3f;

    void Awake()
    {
        jammer = GetComponent<Jammer>();
    }

    public void Activate()
    {
        activated = true;
    }

    public void OnClick()
    {
        if (!activated) return;

        Staff staff = FindObjectOfType<Staff>();
        this.AssignStaff(staff);
    }

    #region IStaffAssignation Implementation

    public void AssignStaff(Staff newStaff)
    {
        if (this.assignedStaff != null) throw new System.Exception("There is staff assigned already");

        newStaff.Assign(this);
        assignedStaff = newStaff;
        newStaff.walker.MoveTo(staffPosition, OnStaffReady);
    }

    public void OnStaffReady()
    {
        assignedStaff.walker.TurnTo(this.transform.position);
        Process();
    }

    public void Process()
    {
        StartCoroutine(ProcessCoroutine());
    }

    public void UnassignStaff()
    {
        StopAllCoroutines();
        assignedStaff = null;
    }

    #endregion

    IEnumerator ProcessCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        assignedStaff.Unassign();
        jammer.assignedTable.AddJammer(jammer);
        this.activated = false;
    }
}
