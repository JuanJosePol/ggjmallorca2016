using UnityEngine;
using System;
using System.Collections;

public class WiFiProblem : OnSiteProblem
{

    public override void OnActivate()
    {
        jammer.LoadDialog(DialogType.Wifi);
    }

    public override void OnDeactivate()
    {
        jammer.HideDialog();
    }
}
