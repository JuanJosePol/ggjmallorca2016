using UnityEngine;
using System.Collections;

public abstract class OnSiteProblem : MonoBehaviour, IStaffAssignation
{
    private Vector3 staffPosition { get { return transform.position + transform.forward; } }

    [HideInInspector]
    public Staff assignedStaff;

    protected Jammer jammer;
    private bool _activated;
    public bool activated
    {
        get { return _activated; }
        set
        {
            _activated = value;
            GetComponent<Collider>().enabled = value;
            if (value)
                OnActivate();
            else
                OnDeactivate();
        }
    }
    protected float repairTime = 3f;
    public abstract void OnActivate();
    public abstract void OnDeactivate();

    void Awake()
    {
        jammer = GetComponentInParent<Jammer>();
        activated = false;
    }

    public void OnClick()
    {
        if (!activated) return;

        if (GameManager.instance.selectedStaff != null)
        {
            this.AssignStaff(GameManager.instance.selectedStaff);
        }
    }

    #region IStaffAssignation Implementation

    public void AssignStaff(Staff newStaff)
    {
        if (this.assignedStaff != null) return;

        newStaff.Assign(this);
        assignedStaff = newStaff;
        GetComponent<Collider>().enabled = false;
        newStaff.walker.MoveTo(staffPosition, false, OnStaffReady);
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
        GetComponent<Collider>().enabled = true;
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
