using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Synthesis.ModelManager;
using Synthesis.ModelManager.Models;
using System.Linq;
using Synthesis.UI.Bars;
using Newtonsoft.Json;

public class MyTest : MonoBehaviour
{
    [SerializeField]
    public float HighlightTime;
    
    public Synthesis.Camera.Camera TheCamera;

    [SerializeField]
    public Bar currentBar;

    [SerializeField]
    public GameObject configGearboxPanel;

    private Model Robot;
    private float lastUpdate = 0;
    private int index = 0;

    private void Start()
    {
        ModelManager.OnModelSpawned += m => { Robot = m; Debug.Log($"Model \"{m.Name}\" Spawned"); };
        TheCamera = GameObject.Find("Main Camera").GetComponent<Synthesis.Camera.Camera>();

        // Configure Gearboxes Test
        // currentBar.OpenPanel(configGearboxPanel);

        // Test Serializing GearboxData
        GearboxData data = new GearboxData() { Name = "test1", MotorUuids = new string[] { "0a00", "0a01" }, MaxSpeed = 50, Torque = 2.1f };
        string a = JsonConvert.SerializeObject(data);
        Debug.Log(a);
        var deserialize = JsonConvert.DeserializeObject<GearboxData>(a);
        // Debug.Log("How'd it go?");
    }

    public void FixedUpdate()
    {
        // Look at joints test
        /*if (Time.realtimeSinceStartup - lastUpdate > HighlightTime)
        {
            // Debug.Log($"Running increment: {index}");
            if (Robot != null && Robot.Motors.Count > 0)
            {
                TheCamera.LookAt(Robot.Motors.ElementAt(index % Robot.Motors.Count).Joint.anchor);
                index++;
            }
            lastUpdate = Time.realtimeSinceStartup;
        }*/
    }
}
