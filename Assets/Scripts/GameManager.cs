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

    // Input Selection
    [HideInInspector]
    public Staff selectedStaff;

    public bool HasRoom
    {
        get { return jammers.Count < tables.Count * 4; }
    }

    public void OnClick(MonoBehaviour clicked)
    {
        //if (clicked is Staff)
        //{
        //    Staff s = (Staff)clicked;
        //}

        //if (clicked is Problem)
        //{
        //    Problem p = (Problem) 
        //}
    }
}
