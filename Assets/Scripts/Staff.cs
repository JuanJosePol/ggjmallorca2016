using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Staff : MonoBehaviour
{
    [HideInInspector]
	public Walker walker;
	public float stamina=100;
	public Slider progressSlider;

	[HideInInspector]public IStaffAssignation assignation;
	[HideInInspector]public StaffRenderer staffRenderer;
	GameObject selectionArrow;
	
	static string[] nameArray = {"Sergi Lorenzo", "Juanjo Pol", "Javi Cepa", "Elena Blanes", "Curro Campos", "Alberto Rico", "Alejandro Rico", "David Rico", "Jesús Fernández", "Espe Olea", "Aina Ferriol"};
	static List<string> remainingNames;
    public bool canBeSelected = true;
    private bool _sleeping = false;
    public bool sleeping
    {
        get { return _sleeping; }
        set
        {
            _sleeping = value;
            if (value)
                LayDown();
            else
                StandUp();
        }
    }
    public float assignmentProgress { get; set; }

	void GenerateStaffName() {
		int selection=Random.Range(0,remainingNames.Count);
		name=remainingNames[selection];
		remainingNames.Remove(name);
	}
	
	void Update() {
		if (assignation!=null && !(assignation is Dorm)) {
			progressSlider.gameObject.SetActive(true);
			progressSlider.value+=assignmentProgress;
			stamina-=Time.deltaTime;
		} else {
			stamina += Time.deltaTime * (sleeping ? 2 : 1);
			progressSlider.gameObject.SetActive(false);
			progressSlider.value=0;
		}
		stamina=Mathf.Clamp(stamina, 0, 100);

        if (stamina < 20 && assignation != null && !(assignation is Dorm))
        {
            GoToSleep();
        }

		if (stamina <= 1) {
			Unassign();
		}

        progressSlider.gameObject.SetActive(assignmentProgress > 0);
        progressSlider.value = assignmentProgress;
	}
	
    void Awake()
    {
	    GameManager.instance.staff.Add(this);
	    remainingNames=new List<string>(nameArray);
	    walker = gameObject.AddComponent<Walker>();
	    staffRenderer=GetComponentInChildren<StaffRenderer>();
	    selectionArrow=GetComponentInChildren<Bounce>().gameObject;
	    selectionArrow.SetActive(false);
        assignmentProgress = -1;
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
	    }
    }

    public void Unassign()
    {
        if (assignation != null)
        {
            ((MonoBehaviour)assignation).SendMessage("WakeUp", this, SendMessageOptions.DontRequireReceiver);
            assignation.UnassignStaff();
            assignation = null;
            assignmentProgress = -1;
        }
    }

    public void OnClick()
	{
		AssetCatalog.instance.PlaySound("select");
        GameManager.instance.SelectStaff(this);
    }

    private void GoToSleep()
    {
        Dorm d = GameManager.instance.dorm;
        if (d != null && d.hasRoom)
        {
            Unassign();
            d.AssignStaff(this);
        }
    }

    private void LayDown()
    {
        walker.navAgent.enabled = false;
        transform.DOLocalRotate(new Vector3(1, 0, 1000), 1, RotateMode.Fast).SetEase(Ease.InOutQuint);
    }

    private void StandUp()
    {
        walker.navAgent.enabled = true;
        //transform.DOLocalRotate(new Vector3(-1,0,-1000), 1, RotateMode.Fast);
    }
}
