using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody cubeRigidBody;
    private Vector3 inputVector;

    private float moveX;
    private float moveY;

    void Start()
    {
        cubeRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    	// GetAxis zero to one progressively
    	// GetAxisRaw zero to one snap directly
    	print(Input.GetAxis("Horizontal"));

    	// multiplier factor is __ x faster than gravity; y is gravity
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal") * 5, cubeRigidBody.velocity.y, Input.GetAxis("Vertical") * 5);
        
        // transform.position is the start + 
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
        cubeRigidBody.velocity = inputVector;
    }
}
