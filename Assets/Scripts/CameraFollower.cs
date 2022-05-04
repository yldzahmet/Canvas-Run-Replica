using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    internal static bool isFinished = false;
    public GameObject target;
    private Vector3 offsetVector;
    private Vector3 camPos;
    public Vector3 targetPos;
    public float camRotX = 60;

    // Start is called before the first frame update
    void Start()
    {
        offsetVector = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isFinished)
            camPos = target.transform.position + offsetVector;
        else
        {
            camPos = Vector3.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(camRotX, 0, 0), 10f *Time.deltaTime);
        }
        transform.position = camPos;
    }
}
