using UnityEngine;
using System.Collections;

public class JammerDialog : MonoBehaviour {
	
	SpriteRenderer spriteRenderer;
	
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
	}
	
	public void LoadDialog(DialogType type) {
        if (spriteRenderer != null)
		spriteRenderer.sprite = AssetCatalog.instance.dialogs.GetByName("Dialog"+type);
	}

    public void HideDialog()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = null;
    }
}

public enum DialogType {Food, Question, Sleep, Ticket, Troll, WC, Wifi}