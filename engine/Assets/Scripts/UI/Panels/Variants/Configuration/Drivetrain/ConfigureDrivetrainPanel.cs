using Synthesis.UI.Panels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfigureDrivetrainPanel : Panel
{
    public TMP_Dropdown ModelDropdown;
    public TMP_Dropdown DrivetrainType;

    public List<DrivetrainSubPanel> typePanels;

    private void Start()
    {
        List<string> names = new List<string>();
        typePanels.ForEach(x => names.Add(x.name));
        DrivetrainType.AddOptions(names);

        // TODO: Load drivetrain choice if one exists, along with all the other stuff
        DrivetrainType.onValueChanged.AddListener(x =>
        {
            typePanels.ForEach(y => y.gameObject.SetActive(false)); // Lazy
            typePanels[x].gameObject.SetActive(true);
        });

        typePanels[DrivetrainType.value].gameObject.SetActive(true);
    }
}
