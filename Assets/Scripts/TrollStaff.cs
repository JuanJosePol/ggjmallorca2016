using UnityEngine;
using System.Collections;
using System;

public class TrollStaff : OnSiteProblem
{
    public override void OnActivate()
    {
        jammer.LoadDialog(DialogType.Troll);
    }

    public override void OnDeactivate()
    {
        jammer.HideDialog();
    }
}
