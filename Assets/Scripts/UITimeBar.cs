using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITimeBar : MonoBehaviour {
	
	Slider slider;
	Image image;
	
	void Start () {
		slider=GetComponent<Slider>();
		image=slider.fillRect.GetComponent<Image>();
	}
	
	void Update () {
		slider.value=1f-LevelManager.instance.elapsedLevelTime/LevelManager.instance.levelDuration;
		image.color=HSBColor.Lerp(new HSBColor(Color.red*0.5f), new HSBColor(Color.green*0.5f), Mathf.Clamp01(slider.value*slider.value)).ToColor();
	}
}
