﻿using UnityEngine;
using System;
using System.Collections;

public class WiFiProblem : OnSiteProblem
{

    public override void OnActivate()
    {
        Debug.Log("WiFi Problem Activated");
    }

    public override void OnDeactivate()
    {
        Debug.Log("WiFi Problem Deactivated");
    }
}
