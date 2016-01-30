using UnityEngine;
using System.Collections;

public class Staff : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;

    void Awake()
    {
        GameManager.instance.staff.Add(this);
        walker = gameObject.AddComponent<Walker>();
    }
}
