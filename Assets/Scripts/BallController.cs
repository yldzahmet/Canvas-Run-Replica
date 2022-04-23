using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject head;
    public float speed;
    public float rotationSpeed;
    public float verticalInput = 1;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get the users horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // move the head object forward at a constant rate
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // tilt the head based on left/right arrow keys
        transform.Rotate(Vector3.up * rotationSpeed * horizontalInput * Time.deltaTime);
    }
}
