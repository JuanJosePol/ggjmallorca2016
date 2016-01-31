using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoSlotController : MonoBehaviour {
	
	Slider slider;
	Text   label;
	RawImage image;
	
	public Game  gameInfo;
	public Staff staffInfo;
	
	void Start () {
		slider=GetComponentInChildren<Slider>();
		label=GetComponentInChildren<Text>();
		image=GetComponentInChildren<RawImage>();
		transform.localScale=Vector3.one;
	}
	
	void Update () {
		if (gameInfo!=null) {
			slider.value=gameInfo.progress;
			label.text=gameInfo.name;
			image.texture=AssetCatalog.instance.gameCovers.GetRandom();
		}
		if (staffInfo!=null) {
			slider.value=staffInfo.stamina/100f;
			label.text=staffInfo.name;
			image.texture=staffInfo.staffRenderer.cameraTexture;
		}
	}
}
