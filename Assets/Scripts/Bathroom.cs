using UnityEngine;
using System.Collections;

public class Bathroom : MonoBehaviour, IStaffAssignation
{
    public float breakChance = 0.25f;
    public float poopTime = 5;
    public float repairTime = 4;
    public Transform enterPosition;
    public Transform poopPosition;
    public WaitingQueue waitingQueue;
    public MeshFilter WCMeshFilter;
    public MeshFilter ClosedWC;
    public MeshFilter OpenWC;
    public MeshRenderer puddle;

    [HideInInspector]
    public Jammer jammer;
    
    private bool _isBroken;
    public bool isBroken
    {
        get { return _isBroken; }
        set
        {
            _isBroken = value;
            puddle.enabled = value;
        }
    }

    void Awake()
    {
        GameManager.instance.bathrooms.Add(this);
        WCMeshFilter.mesh = OpenWC.sharedMesh;
        isBroken = false;
    }

    public bool CanEnterJammer()
    {
        return jammer == null || !waitingQueue.isFull;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (!CanEnterJammer()) throw new System.Exception("No more jammers allowen in Bathroom.");

        if (this.jammer == null)
        {
            this.jammer = newJammer;
            jammer.walker.MoveTo(enterPosition.position, true, OnJammerReady);
        }
        else
        {
            waitingQueue.AddJammer(newJammer);
        }
    }

    private void OnJammerReady()
    {
        Process();
    }

    public void Process()
    {
        if (!isBroken)
        {
            // Enter the bathroom
            jammer.HideDialog();
            jammer.walker.MoveTo(poopPosition.position, false, () => { StartCoroutine(UseBathroom()); });
        }
        else
        {
            jammer.LoadDialog(DialogType.WC);
        }
    }

    public IEnumerator UseBathroom()
    {
        WCMeshFilter.mesh = ClosedWC.sharedMesh;
        float timeLeft = poopTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        ExitBathroom();
    }

    public void ExitBathroom()
	{
		
		AssetCatalog.instance.PlaySound("wc");
        if (UnityEngine.Random.value < breakChance)
            this.isBroken = true;

        WCMeshFilter.mesh = OpenWC.sharedMesh;
        jammer.walker.MoveTo(enterPosition.position, false, () => { FreeBathroom(); });
    }

    public void FreeBathroom()
    {
        jammer.assignedTable.AddJammer(jammer);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Jammer>() == jammer)
        {
            jammer = null;
            if (!waitingQueue.isEmpty)
                AddJammer(waitingQueue.GetNextJammer());
        }
    }

    #region IStaffAssignation Implementation

    [HideInInspector]
    public Staff assignedStaff;

    public Transform staffPosition;

    public void AssignStaff(Staff newStaff)
    {
        if (this.assignedStaff != null)
            return;
        
        newStaff.Assign(this);
        assignedStaff = newStaff;
        assignedStaff.walker.MoveTo(staffPosition.position, false, OnStaffReady);
    }

    public void UnassignStaff()
    {
        StopAllCoroutines();
        assignedStaff = null;
    }

    public void OnStaffReady()
    {
        assignedStaff.walker.MoveTo(poopPosition.position, false, () => { StartCoroutine(FixCoroutine()); });
    }

    public void OnClick()
    {
        if (GameManager.instance.selectedStaff != null)
        {
            this.AssignStaff(GameManager.instance.selectedStaff);
        }
    }

    #endregion

    IEnumerator FixCoroutine()
    {
        WCMeshFilter.mesh = ClosedWC.sharedMesh;

        float timeElapsed = 0;
        while (timeElapsed < repairTime)
        {
            timeElapsed += Time.deltaTime;
            assignedStaff.assignmentProgress = timeElapsed / repairTime;
            yield return null;
        }
        assignedStaff.assignmentProgress = -1;

        isBroken = false;
        WCMeshFilter.mesh = OpenWC.sharedMesh;

        assignedStaff.walker.MoveTo(staffPosition.position, false, FinishRepairs);
    }

    private void FinishRepairs()
    {
        assignedStaff.Unassign();
        if (jammer != null)
            Process();
    }
}
