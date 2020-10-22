using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Camera
{
    public class Camera : MonoBehaviour
    {
        private Transform transform;

        // Start is called before the first frame update
        void Start()
        {
            transform = this.gameObject.GetComponent<Transform>();
            transform.LookAt(new Vector3(0, 0, 0), Vector3.up);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

