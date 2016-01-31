using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoSlotController : MonoBehaviour {
	
	Slider slider;
	Text   label;
	RawImage image;
	Text   sliderLabel;
	
	public Game  gameInfo;
	public Staff staffInfo;
	
	void Start () {
		slider=GetComponentInChildren<Slider>();
		label=transform.FindChild("Text").GetComponent<Text>();
		image=GetComponentInChildren<RawImage>();
		sliderLabel=slider.GetComponentInChildren<Text>();
		transform.localScale=Vector3.one;
	}
	
	void Update () {
		if (gameInfo!=null) {
			slider.value=gameInfo.progress;
			label.text=gameInfo.name;
			if (gameInfo.progress<1) {
				sliderLabel.text=Mathf.RoundToInt(gameInfo.progress*100)+"%";
			} else {
				sliderLabel.text="Done!";
			}
			if (image.texture==null) {
				image.texture=AssetCatalog.instance.gameCovers.GetRandom();
			}
		}
		if (staffInfo!=null) {
			slider.value=staffInfo.stamina/100f;
			label.text=staffInfo.name;
			sliderLabel.text=""+Mathf.RoundToInt(staffInfo.stamina);
			image.texture=staffInfo.staffRenderer.cameraTexture;
		}
	}
}
