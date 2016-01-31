using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class InfoSlotController : MonoBehaviour {
	
	Slider slider;
	Text   label;
	RawImage image;
	Text   sliderLabel;
	
	public Game  gameInfo;
	public Staff staffInfo;
	
	bool isDone=false;
	
	void Start () {
		slider=GetComponentInChildren<Slider>();
		label=transform.FindChild("Text").GetComponent<Text>();
		image=GetComponentInChildren<RawImage>();
		sliderLabel=slider.GetComponentInChildren<Text>();
		transform.localScale=Vector3.zero;
		transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic);
	}
	
	void Update () {
		if (gameInfo!=null) {
			slider.value=gameInfo.progress;
			label.text=gameInfo.name;
			if (gameInfo.progress<1) {
				sliderLabel.text=Mathf.RoundToInt(gameInfo.progress*100)+"%";
			} else {
				if (!isDone) {
					isDone=true;
					sliderLabel.text="Done!";
					LevelManager.instance.createdGames++;
					transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InElastic).SetDelay(0.5f).OnComplete(()=>Destroy(gameObject, 1));
				}
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
