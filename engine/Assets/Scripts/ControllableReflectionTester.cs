using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Synthesis.ModelManager.Controllable;

public class ControllableReflectionTester : MonoBehaviour {

    public void Start() {
        var c = new TestControllable();
        var inputs = Controllable.GetInputProperties(c);
        foreach (var input in inputs) {
            Debug.Log($"{input.Key} => {input.Value.Item1(c)}");
            // input.Value.Item2(c, input.Value.Item1(c) * 2);
        }

        // Debug.Log($"TestAxisOne : {c.TestAxisOne}");
        // Debug.Log($"TestAxisTwo : {c.TestAxisTwo}");
    }

}
