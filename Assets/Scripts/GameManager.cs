<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    #endregion

    // Game Objects lists
    [HideInInspector]
    public List<Table> tables = new List<Table>();
    [HideInInspector]
    public List<Jammer> jammers = new List<Jammer>();
    [HideInInspector]
    public List<Staff> staff = new List<Staff>();
    [HideInInspector]
    public List<Bathroom> bathrooms = new List<Bathroom>();
    
    public List<Game> games = new List<Game>();
    public int space = 0;

    public bool hasRoomForJammers
    {
        get
        {
            return jammers.Count < space;
        }
    }

    void Start()
    {
        foreach (var t in tables)
        {
            space += t.chairPosition.Length;
        }
    }
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.N)) {
			Debug.Log(GenerateGameName());
		}
	}

    public Game CreateNewGame()
    {
	    Game game = new Game(GenerateGameName(), 50+Random.Range(0, 50));
        games.Add(game);
        return game;
    }
	
	string[] firstGameNameWords = {"Super", "Metro",  "Infini", "Mine"     , "Nom",     "Mini",  "Over"   , "Picco",     "Triangle", "Another",		"Call of",		"Attack of", "Looking for", "Jammer", "The Legend of", "Into the", "Fart", "Epic", "Academy of", "Violence", "Pro", "Ultra", "Tech", "Nerd", "Annoying", "Gizmo", "Dinosaur", "Into the", "Love", "Goat", "Penguin", "Dog", "Masters of"};
	string[] secondGameNameWords= {"Bario", "Noid",   "Fun",    "Draft"    , "Om",      "Creed", "Toto"   , "Lovers",    "Heroes",    "Ritual",		"Dummies",		"Revenge", "Sensations", "Drums", "Balls", "Drugs", "Problems", "Dislexia", "Monogamy", "Manager", "Tycoon", "Hair", "Rock", "Vegan", "Jam", "Pop", "Hero", "Pizza"};
	string[] thirdGameNameWords = {"Gloss", "Fission","Plus",   "Adventure", "Legends", "Croc",  "Oddisey", "Obsession", "2016"    , ": The Game",	"Chronicles",	"Returns", "Revival", "2", "3", "X", "& Coffee", "& Luigi", "Redemption", "Revelations", "Evolution", "Kart", "Neighbour", "Simulator", "Friends", "Friends"};
	
	
	public string GenerateGameName() {
		string name="";
		name+=firstGameNameWords.GetRandom()+" ";
		if (Random.value>0.5f) {
			name+=secondGameNameWords.GetRandom()+" ";
			if (Random.value>0.25f) {
				name+=thirdGameNameWords.GetRandom()+" ";
			}
		} else {
			name+=thirdGameNameWords.GetRandom()+" ";
		}
		return name;
	}
	
}
=======
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    #endregion

    public float troubleFrequency = 0.1f;

    // Game Objects lists
    [HideInInspector]
    public List<Table> tables = new List<Table>();
    [HideInInspector]
    public List<Jammer> jammers = new List<Jammer>();
    [HideInInspector]
    public List<Staff> staff = new List<Staff>();
    [HideInInspector]
    public List<Bathroom> bathrooms = new List<Bathroom>();
    [HideInInspector]
    public List<Game> games = new List<Game>();

    private int space = 0;
    private float timeSinceLastProblem = 0;

    public bool hasRoomForJammers
    {
        get
        {
            return jammers.Count < space;
        }
    }

    void Start()
    {
        foreach (var t in tables)
        {
            space += t.chairPosition.Length;
        }
    }
    
    void Update()
    {
        timeSinceLastProblem += Time.deltaTime;

        if (timeSinceLastProblem > 1 / troubleFrequency)
        {
            timeSinceLastProblem = 0;

            GenerateBathroomProblem();
        }
    }

    private void GenerateBathroomProblem()
    {
        bool freeBathroom = false;
        foreach (var b in bathrooms)
        {
            if (b.CanEnterJammer())
            {
                freeBathroom = true;
                break;
            }
        }
        if (!freeBathroom) return;

        Jammer troubledJammer = PickRandomJammer();
        if (troubledJammer != null)
            troubledJammer.FindFreeBathroom();

    }

    private Jammer PickRandomJammer()
    {
        List<Jammer> workingJammers = new List<Jammer>();
        foreach (var j in this.jammers)
        {
            if (j.isWorking)
                workingJammers.Add(j);
        }

        if (workingJammers.Count == 0) return null;

        return workingJammers[Random.Range(0, workingJammers.Count)];
    }

    public Game CreateNewGame()
    {
        Game game = new Game("AAA", 20);
        games.Add(game);
        return game;
    }
}
>>>>>>> 22d4a2bf805e9ce87cacfd6f1e3f5b867ecf6374
