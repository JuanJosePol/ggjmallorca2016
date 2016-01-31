using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FoodZone : MonoBehaviour, IStaffAssignation
{
    private int MaxFoodRations = 7;
    public float eatTime = 3;
    public float phoneCallTime = 3;
    public float buyFoodTime = 3;
    public Transform eatingPosition;
    public Transform restockFoodPosition;
    public WaitingQueue waitingQueue;
    public List<MeshFilter> pizzasOnTable;
    public List<MeshFilter> pizza1Frames;
    public List<MeshFilter> pizza2Frames;

    [HideInInspector]
    public Jammer jammer;

    private int _foodRations;
    public int foodRations
    {
        get { return _foodRations; }
        set
        {
            _foodRations = value;
            UpdatePizzas();
        }
    }

    void Awake()
    {
        GameManager.instance.foodZones.Add(this);
	    foodRations = MaxFoodRations / 3;
    }

    private void UpdatePizzas()
    {
        if (foodRations >= 4)
        {
            pizzasOnTable[0].mesh = pizza1Frames[4].sharedMesh;
            pizzasOnTable[1].mesh = pizza2Frames[foodRations - 4].sharedMesh;
        }
        else
        {
            pizzasOnTable[0].mesh = pizza1Frames[foodRations].sharedMesh;
            pizzasOnTable[1].mesh = pizza2Frames[0].sharedMesh;
        }
    }

    public bool CanEnterJammer()
    {
        return !waitingQueue.isFull;
    }

    public void AddJammer(Jammer newJammer)
    {
        if (!CanEnterJammer()) throw new System.Exception("No more jammers allowed in FoodZone");

        if (this.jammer == null)
        {
            this.jammer = newJammer;
            jammer.walker.MoveTo(eatingPosition.position, true, OnJammerReady);
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
        if (foodRations > 0)
        {
            StartCoroutine(EatCoroutine());
        }
        else
        {
            jammer.LoadDialog(DialogType.Food);
        }
    }

    IEnumerator EatCoroutine()
    {
        yield return new WaitForSeconds(eatTime);
        foodRations -= 1;
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
	    AssetCatalog.instance.PlaySound("task");
	    assignedStaff.walker.MoveTo(staffPosition.position, false, OnStaffReady);
    }

    public void UnassignStaff()
    {
        StopAllCoroutines();
        assignedStaff = null;
    }

    public void OnStaffReady()
    {
        assignedStaff.walker.TurnTo(staffPosition.position + staffPosition.forward);
        StartCoroutine(MakePhoneCall());
    }

    public void OnClick()
    {
        if (GameManager.instance.selectedStaff != null)
        {
            this.AssignStaff(GameManager.instance.selectedStaff);
        }
    }

    #endregion

    IEnumerator MakePhoneCall()
    {
	    
	    AssetCatalog.instance.PlaySound("phone");
	    float timeElapsed = 0;
        while (timeElapsed < phoneCallTime)
        {
            timeElapsed += Time.deltaTime;
            assignedStaff.assignmentProgress = timeElapsed / phoneCallTime;
            yield return null;
        }
        assignedStaff.assignmentProgress = -1;

	    assignedStaff.canBeSelected = false;
	    if (GameManager.instance.selectedStaff==assignedStaff) {
	    	GameManager.instance.DeselectStaff();
	    }

        assignedStaff.walker.MoveTo(FindObjectOfType<JammerGenerator>().transform.position, false, () => { StartCoroutine(GoBuyFood()); });
    }

    IEnumerator GoBuyFood()
    {
        yield return new WaitForSeconds(buyFoodTime);

        assignedStaff.walker.MoveTo(restockFoodPosition.position, false, RestockFood);
    }

    private void RestockFood()
    { 
        bool needHelp = foodRations == 0;
        foodRations = MaxFoodRations;
        assignedStaff.canBeSelected = true;
        assignedStaff.Unassign();
        if (jammer != null && needHelp)
            Process();
    }
}
