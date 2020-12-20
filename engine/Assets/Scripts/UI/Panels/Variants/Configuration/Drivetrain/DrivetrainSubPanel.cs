using Synthesis.ModelManager.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivetrainSubPanel : MonoBehaviour
{
    public bool IsShown { get; set; }
    protected Model selectedModel { get; set; }

    public virtual void Show(Model selectedModel) {
        IsShown = true;
        this.selectedModel = selectedModel;
        gameObject.SetActive(true);
    }
    public virtual void Hide() {
        IsShown = false;
        this.selectedModel = null;
        gameObject.SetActive(false);
    }
}
