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

    public Game CreateNewGame()
    {
        Game game = new Game("AAA", 20);
        games.Add(game);
        return game;
    }
}
