#pragma warning disable 0618

using UnityEngine;
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
    public List<FoodZone> foodZones = new List<FoodZone>();
    [HideInInspector]
    public Dorm dorm;
    [HideInInspector]
    public List<Game> games = new List<Game>();

    private int space = 0;
    private float timeSinceLastProblem = 0;
    private int problemTypeCount = 2; // WiFi and Troll are always possible;
    private AudioSource crowdAudio;

    public bool hasRoomForJammers
    {
        get
        {
            return jammers.Count < space;
        }
    }
	
	void Awake() {
		Application.LoadLevelAdditive("UI");

        if (FindObjectOfType<Bathroom>() != null)
        {
            problemTypeCount += 1;
        }

        if (FindObjectOfType<FoodZone>() != null)
        {
            problemTypeCount += 1;
        }

        crowdAudio = Camera.main.GetComponent<AudioSource>();
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

            float problemType = UnityEngine.Random.Range(0, problemTypeCount);
            if (problemType == 0)
                GenerateWiFiProblem();
            else if (problemType == 1)
                GenerateTrollStaff();
            else if (problemType == 2)
                GenerateBathroomProblem();
            else if (problemType == 3)
                GenerateFoodProblem();   
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo) || (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Clickable")))
            {
                DeselectStaff();
            }
        }

        if (crowdAudio != null)
        {
            float t = 0; ;
            if (Application.loadedLevel == 0)
                t = Mathf.Max(0, (jammers.Count - 4) / (float)space);
            if (Application.loadedLevel == 1)
                t = Mathf.Max(0, (jammers.Count - 10) / 40f);
            if (Application.loadedLevel == 3)
                t = Mathf.Max(0, (jammers.Count - 5) / 40f);
            
            crowdAudio.volume = Mathf.Lerp(0, 1, t);
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

        Jammer troubledJammer = PickRandomWorkingJammer();
        if (troubledJammer != null)
            troubledJammer.FindFreeBathroom();
    }

    private void GenerateWiFiProblem()
    {
        Jammer troubledJammer = PickRandomWorkingJammer();
        if (troubledJammer != null)
        {
            List<Jammer> table = new List<Jammer>();
            table.AddRange(troubledJammer.assignedTable.jammers);
            foreach (Jammer j in table)
            {
                j.HaveWiFiProblem();
            }
        }
    }

    private void GenerateTrollStaff()
    {
        Jammer troubledJammer = PickRandomWorkingJammer();
        if (troubledJammer != null)
            troubledJammer.TrollStaff();
    }

    private void GenerateFoodProblem()
    {
        Jammer troubledJammer = PickRandomWorkingJammer();
        if (troubledJammer != null)
            troubledJammer.GoGetFood();
    }

    private Jammer PickRandomWorkingJammer()
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
	    Game game = new Game(GenerateGameName(), 150);
        games.Add(game);
        return game;
    }
	
	string[] firstGameNameWords = {"Super", "Metro",  "Infini", "Mine"     , "Nom",     "Mini",  "Over"   , "Picco",     "Triangle", "Another",		"Call of",		"Attack of", "Looking for", "Jammer", "The Legend of", "Into the", "Fart", "Epic", "Academy of", "Violence", "Pro", "Ultra", "Tech", "Nerd", "Annoying", "Gizmo", "Dinosaur", "Into the", "Love", "Goat", "Penguin", "Dog", "Masters of", "League of"};
	string[] secondGameNameWords= {"Bario", "Noid",   "Fun",    "Draft"    , "Om",      "Creed", "Toto"   , "Lovers",    "Heroes",    "Ritual",		"Dummies",		"Revenge", "Sensations", "Drums", "Balls", "Drugs", "Problems", "Dislexia", "Monogamy", "Manager", "Violence", "Tycoon", "Hair", "Rock", "Vegan", "Jam", "Pop", "Hero", "Pizza"};
	string[] thirdGameNameWords = {"Gloss", "Fission","Plus",   "Adventure", "Legends", "Croc",  "Oddisey", "Obsession", "2016"    , ": The Game",	"Chronicles",	"Returns", "Revival", "2", "3", "X", "& Coffee", "& Luigi", "Redemption", "Revelations", "Evolution", "Kart", "Neighbour", "Simulator", "Friends"};
	
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

    public Staff selectedStaff { get; private set; }

    public void SelectStaff(Staff staff)
    {
        if (selectedStaff != null && !selectedStaff.canBeSelected)
            return;

        if (selectedStaff != null)
            selectedStaff.Deselect();

        selectedStaff = staff;
        selectedStaff.Select();
    }

    public void DeselectStaff()
    {
        if (selectedStaff != null)
            selectedStaff.Deselect();

        selectedStaff = null;
    }
}