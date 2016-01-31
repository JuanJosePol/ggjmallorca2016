using UnityEngine;
using System.Collections;
using DG.Tweening;

public class JammerDialog : MonoBehaviour {
	
	SpriteRenderer spriteRenderer;
	
	Vector3 startScale;
	
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = null;
		startScale=transform.localScale;
	}
	
	public void LoadDialog(DialogType type) {
		if (spriteRenderer != null) {
			spriteRenderer.sprite = AssetCatalog.instance.dialogs.GetByName("Dialog"+type);
			transform.localScale=Vector3.zero;
			transform.DOScale(startScale, 2f).SetEase(Ease.OutElastic);
			transform.DOPunchPosition(transform.up, 0.1f);
		}
	}

    public void HideDialog()
    {
	    //if (spriteRenderer != null) {
		//    spriteRenderer.sprite = null;
	    //}
	    transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InElastic);
    }
}

public enum DialogType {Food, Question, Sleep, Ticket, Troll, WC, Wifi}