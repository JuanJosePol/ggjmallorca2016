using UnityEngine;
using System.Collections;

public class Staff : MonoBehaviour
{

    void Awake()
    {
        GameManager.instance.staff.Add(this);
    }

    public void OnClick()
    {
        GameManager.instance.selectedStaff = this;
    }
}
