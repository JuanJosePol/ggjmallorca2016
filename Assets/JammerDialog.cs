using UnityEngine;
using System.Collections;

public class JammerDialog : MonoBehaviour {
	
	SpriteRenderer spriteRenderer;
	
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
	}
	
	public void LoadDialog(DialogType type) {
		spriteRenderer.sprite = AssetCatalog.instance.dialogs.GetByName("Dialog"+type);
	}

    public void HideDialog()
    {
        spriteRenderer.sprite = null;
    }
}

public enum DialogType {Food, Question, Sleep, Ticket, Troll, WC, Wifi}