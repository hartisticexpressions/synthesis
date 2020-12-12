using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Synthesis.ModelManager;

public class AddItem : MonoBehaviour
{
    [SerializeField]
    Text _name;

    private string _fullPath;

    public void AddModel()
    {
        ModelManager.AddModel(_fullPath);
    }

    public void AddField()
    {
        ModelManager.SetField(_fullPath);
    }

    public void Init(string name, string path)
    {
        _name.text = name;
        _fullPath = path;
    }
}
