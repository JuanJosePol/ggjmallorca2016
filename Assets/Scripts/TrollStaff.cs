using UnityEngine;
using System.Collections;
using System;

public class TrollStaff : OnSiteProblem
{
    public override void OnActivate()
    {
        Debug.Log("TollStaff Activated");
    }

    public override void OnDeactivate()
    {
        Debug.Log("TrollStaff Deactivated");
    }
}
