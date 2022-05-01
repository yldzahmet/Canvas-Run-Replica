using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : MonoBehaviour
{
    [SerializeField]
    public static float forwardSpeed = 20;
    public float horizontalSpeed;
    public float maxRotAngle;
    private float rotationAngle;
    private float horizontalInput;

    private Quaternion steer = Quaternion.identity;
    private Vector3 rotationVector = new Vector3();
    private Vector3 currentPos = new Vector3();

    public void RotateHead()
    {
        rotationAngle = Mathf.Lerp(rotationAngle, horizontalInput * maxRotAngle, 60 * Time.deltaTime);
        rotationVector.y = rotationAngle;
        steer.eulerAngles = rotationVector;

        // tilt the head based on left/right arrow keys
        transform.rotation = steer;
    }

    public void ClampPosition()
    {
        currentPos = transform.position;
        if (transform.position.x < -5.0f)
        {
            currentPos.x = -5;
            transform.position = currentPos;
        }
        else if (transform.position.x > 5.0f)
        {
            currentPos.x = 5;
            transform.position = currentPos;
        }
    }

    public void DriveBalls()
    {
        // Get the users horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // Move the head object forward at a constant rate
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Move the head object left/right at a constant rate
        transform.Translate(Vector3.right * horizontalSpeed * horizontalInput * Time.deltaTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // If pressed play button
        if (Manager.startDriving)
        {
            DriveBalls();
            ClampPosition();
            RotateHead();
        }
    }
}
