using UnityEngine;
using System.Collections;

public class Workplace : MonoBehaviour {
	
	public GameObject[] codeTools;
	public GameObject[] designTools;
	public GameObject[] musicTools;
	public GameObject[] drawTools;
	
	GameObject activeObject;
	
	JammerType jammerType;
	
	bool claimed=false;
	
	void Start () {
		PickJammerType();
		PickActiveObject();
		
		//TODO: Claim the workplace when the jammer moves there for the first time
		ClaimWorkplace();
	}
	
	void PickJammerType() {
		jammerType=JammerType.Coder;
		if (Random.value>0.5f) {
			jammerType=JammerType.Artist;
			if (Random.value>0.5f) {
				jammerType=JammerType.Designer;
				if (Random.value>0.5f) {
					jammerType=JammerType.Musician;
				}
			}
		}
	}
	
	void PickActiveObject() {
		switch (jammerType) {
		case JammerType.Coder:    activeObject=codeTools.GetRandom();   break;
		case JammerType.Artist:   activeObject=drawTools.GetRandom();   break;
		case JammerType.Designer: activeObject=designTools.GetRandom(); break;
		case JammerType.Musician: activeObject=musicTools.GetRandom();  break;
		default:				  activeObject=codeTools.GetRandom();   break;
		}
	}
	
	public void ClaimWorkplace() {
		if (!claimed) {
			claimed=true;
			activeObject.SetActive(true);
		}
	}
	
	void Update () {
		
	}
}

public enum JammerType {Coder, Artist, Designer, Musician}