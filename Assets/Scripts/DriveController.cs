using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : MonoBehaviour
{
    public float forwardSpeed = 20;
    private float horizontalInput;
    public float horizontalSpeed = 20;
    private float rotationAngle;
    public float maxRotationAngle;
    public static float rightBorder;

    internal Rigidbody rigidBody;

    private Quaternion steer = Quaternion.identity;
    private Vector3 rotationVector = new Vector3();
    private Vector3 currentPos = new Vector3();

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void RotateHead()
    {
        rotationAngle = Mathf.Lerp(rotationAngle, horizontalInput * maxRotationAngle, 100 * Time.deltaTime);
        rotationVector.y = rotationAngle;
        steer.eulerAngles = rotationVector;
        transform.rotation = steer;
    }

    // Arrange position limits of both side of road
    public float SetBorders(int currentWidth)
    {
        return 10.15f - 0.35f * currentWidth;
    }

    public void ClampPosition()
    {
        rightBorder = SetBorders(PoolController.currentWidth);
        currentPos = rigidBody.transform.position;
        if (rigidBody.transform.position.x < -rightBorder)
        {
            currentPos.x = -rightBorder;
            rigidBody.transform.position = currentPos;
        }
        else if (transform.position.x > rightBorder)
        {
            currentPos.x = rightBorder;
            rigidBody.transform.position = currentPos;
        }
    }

    public void DriveBalls()
    {
        // Get the users horizontal
        horizontalInput = Input.GetAxis("Horizontal");
        Vector3 directionyVector = new Vector3(horizontalInput * horizontalSpeed * Time.deltaTime, 0, forwardSpeed * Time.deltaTime);
        print("Direction Vector : " + directionyVector);
        transform.Translate(directionyVector);
    }

    // Update is called once per frame
    void Update()
    {   
        //If pressed play button
        if (Manager.startDriving)
        {
            ClampPosition();
            DriveBalls();
            RotateHead();
        }
    }
}
