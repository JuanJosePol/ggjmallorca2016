using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamesCounter : MonoBehaviour {
	
	Text text;
	
	void Start () {
		text=GetComponent<Text>();
	}
	
	void Update () {
		text.text="Games: "+LevelManager.instance.createdGames;
	}
}
