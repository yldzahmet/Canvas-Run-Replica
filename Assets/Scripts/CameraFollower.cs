using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;
    private Vector3 offsetVector;

    // Start is called before the first frame update
    void Start()
    {
        offsetVector = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + offsetVector;
    }
}
