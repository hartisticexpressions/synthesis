using Synthesis.ModelManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadePanel : DrivetrainSubPanel
{
    public TMP_Dropdown Left_Gearbox;
    public TMP_Dropdown Right_Gearbox;

    public override void Show(Model selectedModel)
    {
        base.Show(selectedModel);

        List<string> gearboxes = new List<string>();
        gearboxes.Add("None");
        selectedModel.GearboxMeta.ForEach(x => gearboxes.Add(x.Name));
    }

    public override void Hide()
    {
        base.Hide();
    }
}
