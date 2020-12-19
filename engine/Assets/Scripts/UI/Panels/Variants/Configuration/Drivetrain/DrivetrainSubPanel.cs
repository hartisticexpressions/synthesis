using Synthesis.ModelManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivetrainSubPanel : MonoBehaviour
{
    public bool IsShown { get; set; }

    protected virtual void Show(Model selectedModel) => IsShown = true;
    protected virtual void Hide() => IsShown = false;
}
