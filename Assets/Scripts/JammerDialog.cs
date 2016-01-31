using UnityEngine;
using System.Collections;

public class JammerDialog : MonoBehaviour {
	
	SpriteRenderer spriteRenderer;
	
	void Start () {
		spriteRenderer=GetComponent<SpriteRenderer>();
	}
	
	public void LoadDialog(DialogType type) {
		spriteRenderer.sprite=AssetCatalog.instance.dialogs.GetByName("Dialog"+type.ToString());
	}
}

public enum DialogType {Food, Question, Sleep, Ticket, Troll, WC, Wifi}