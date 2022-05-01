using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Follower : MonoBehaviour
{
    internal PoolController poolController;
    [SerializeField]
    // Gameobject to following
    internal GameObject head;
    // Is this object currently following head
    internal bool isFollowing = true;
    // Const offset between balls
    internal float delta = DriveController.forwardSpeed;

    private void Awake()
    {
        poolController = GameObject.Find("Controller").GetComponent<PoolController>();
    }

    // Hide object and make ready to use again
    public void ReturnToPool()
    {
        if (head.CompareTag("Driver"))
        {
            // If this object following Driver object
            head.GetComponent<DriversManager>().isPullingBall = false;
        }
        isFollowing = false;
        poolController.InsertToPool(gameObject);
        transform.localPosition = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }

    //  Calling from update every frame
    public void FollowHead()
    {
        transform.position = Vector3.Lerp(transform.position, head.transform.position, delta * Time.deltaTime);
    }

    public void GoThrough()
    {
        // move the head object forward at a constant rate
        transform.Translate(Vector3.forward * DriveController.forwardSpeed/2 * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        // If pressed play button
        if (Manager.startDriving){
            if (isFollowing)
            {
                FollowHead();
            }
        }

        // If our head object is not active in hierarchy then go to pool
        if (!head.activeInHierarchy)
        {
            ReturnToPool();
        }
    }
}

