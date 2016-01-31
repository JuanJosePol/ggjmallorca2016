using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TintSlider : MonoBehaviour {
	
	Slider slider;
	Image image;
	
	void Start () {
		slider=GetComponent<Slider>();
		image=slider.fillRect.GetComponent<Image>();
	}
	
	void Update () {
		image.color=HSBColor.Lerp(new HSBColor(Color.red*0.75f), new HSBColor(Color.green*0.75f), Mathf.Clamp01(slider.value*slider.value)).ToColor();
	}
}
