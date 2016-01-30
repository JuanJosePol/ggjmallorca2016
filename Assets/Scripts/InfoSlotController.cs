using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoSlotController : MonoBehaviour {
	
	Slider slider;
	Text   label;
	
	public Game  gameInfo;
	public Staff staffInfo;
	
	void Start () {
		slider=GetComponentInChildren<Slider>();
		label=GetComponentInChildren<Text>();
		transform.localScale=Vector3.one;
	}
	
	void Update () {
		if (gameInfo!=null) {
			slider.value=gameInfo.progress/gameInfo.requieredEffort;
			label.text=gameInfo.name;
		}
		if (staffInfo!=null) {
			slider.value=staffInfo.stamina/100f;
			label.text=staffInfo.name;
		}
	}
}
