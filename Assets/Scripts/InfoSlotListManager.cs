using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfoSlotListManager : MonoBehaviour {
	
	public GameObject infoSlotPrefab;
	
	public static InfoSlotListManager instanceGameList;
	public static InfoSlotListManager instanceStaffList;
	
	public List<InfoSlotController> slots;
	
	public SlotListType slotListType;
	
	void Awake() {
		if (slotListType==SlotListType.Games) {
			instanceGameList=this;
		}
		if (slotListType==SlotListType.Staff) {
			instanceStaffList=this;
		}
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
	}
	
	public void AddStaffSlot(Staff staff) {
		GameObject newSlot=Instantiate(infoSlotPrefab) as GameObject;
		InfoSlotController newSlotcontroller=newSlot.GetComponent<InfoSlotController>();
		newSlotcontroller.staffInfo=staff;
		newSlot.transform.SetParent(transform);
	}
	
	public void AddGameSlot(Game game) {
		GameObject newSlot=Instantiate(infoSlotPrefab) as GameObject;
		InfoSlotController newSlotcontroller=newSlot.GetComponent<InfoSlotController>();
		newSlotcontroller.gameInfo=game;
		newSlot.transform.SetParent(transform);
		newSlot.transform.SetSiblingIndex(0);
	}
}

public enum SlotListType {Games, Staff}