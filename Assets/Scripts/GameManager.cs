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
