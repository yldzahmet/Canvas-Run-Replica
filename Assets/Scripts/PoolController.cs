using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolController : MonoBehaviour
{
    DriversManager driversManager;
    public static int driverCount = 9;
    internal static int currentWidth = 5;
    internal static int currentHeight = 20;
    public int maxHeight = 60;
    public GameObject pooledObjectsParent; // parent object for pooled deactives
    public GameObject ball; // ball prefab

    public List<GameObject> drivers;    // List for holding driver objects
    [SerializeField]
    internal List<GameObject> poolList = new List<GameObject>();    // entire ball objects
    [SerializeField]
    internal List<GameObject> lastBalls = new List<GameObject>( new GameObject[9] );    // Holds balls thats following from rearmost

    void Start()
    {
        driversManager = transform.GetChild(0).GetComponent<DriversManager>();
    }

    // Holds pooled objects under one parent
    public void InsertToPool(GameObject obj)
    {
        obj.transform.SetParent(pooledObjectsParent.transform);
    }

    public enum AdditionType {Column, Row};
    public AdditionType additionType;

    // Expands the structure along height by addition parameter
    public void ExpandHeight(int addition)
    {
        print("ExpandHeight-Start-addition " + addition + " currentHeight " + currentHeight);
        // Iterate through Drivers
        for (int i = 0; i < PoolController.driverCount; i++)
        {
            // If finds one which is active and pulling balls
            if (drivers[i].GetComponent<DriversManager>().isPullingBall)
            {
                // Get one line of sequence
                GetSequenceFromPool(AdditionType.Row, addition, i, currentHeight);
            }
        }
        currentHeight += addition;
        print("ExpandHeight-End-currentHeight " + currentHeight);
    }

    // Collapse the structure along height by substraction parameter
    public void CollapseHeight(int substraction)
    {
        print("CollapseHeight-Start-substraction " + substraction + " currentHeight " + currentHeight);
        // be sure that minimun height always 1
        if (currentHeight - substraction < 1)
        {
            substraction = currentHeight - 1;
            if (substraction == 0)
                return;
        }

        GameObject temp;
        // Iterate through Drivers
        for (int i = 0; i < PoolController.driverCount; i++)
        {
            // If finds one which is active and pulling balls
            if (drivers[i].GetComponent<DriversManager>().isPullingBall)
            {
                // Swipe lastBall as upward as substraction and throw other to pool
                for (int j = 0; j < substraction; j++)
                {
                    temp = lastBalls[i].GetComponent<Follower>().head;
                    lastBalls[i].GetComponent<Follower>().ReturnToPool();
                    lastBalls[i] = temp;
                }   
            }
        }
        currentHeight -= substraction;
        print("CollapseHeight-End-currentHeight " + currentHeight);
    }

    // Expands the structure along width by columnToIncrease parameter
    public void ExpandWidth(int columnToIncrease)
    {
        print("ExpandWidth-Start-columnNumber " + columnToIncrease + " currentWidth " + currentWidth);
        // be sure that maximum columnToIncrease 9
        int limit = columnToIncrease > driverCount - currentWidth ? driverCount - currentWidth : columnToIncrease;

        for(int j = limit; j > 0; )
        {
            // Iterate over drivers inside to outside 
            for (int i = 3; i >= 0; i--)
            {
                // If find left head which is empy
                if (drivers[i].GetComponent<DriversManager>().headProperty == DriversManager.Head.LeftHead)
                {
                    GetSequenceFromPool(AdditionType.Column, columnToIncrease, i, currentHeight);

                    drivers[i].GetComponent<DriversManager>().headProperty = DriversManager.Head.None;
                    currentWidth += 1;
                    print("ExpandWidth- 3 > 0 -End-currentWidth " + currentWidth);
                    j--;
                    break;
                }
            }
            if (j == 0)
                return;

            // // Iterate over drivers outside to inside
            for (int i = 4; i < driverCount ; i++)
            {
                // If find right head which is empy
                if (drivers[i].GetComponent<DriversManager>().headProperty == DriversManager.Head.RightHead)
                {
                    GetSequenceFromPool(AdditionType.Column, columnToIncrease, i, currentHeight);

                    drivers[i].GetComponent<DriversManager>().headProperty = DriversManager.Head.None;
                    currentWidth += 1;
                    print("ExpandWidth- 4 > 8 -End-currentWidth " + currentWidth);
                    j--;
                    break;
                }
            }
            // Detect new left and right head corners
            driversManager.FindCorners();
        }
    }

    // Collapse the structure along width by substraction parameter
    public void CollapseWidth(int substraction)
    {
        print("CollapseWidth-Start-substraction " + substraction + " currentWidth " + currentWidth);

        // Loop substraction times
        for (int j = substraction; j > 0 ; )
        {
            // Iterate over drivers from left to inside
            for (int i = 0 ; i < 4 ; i++)
            {
                // If finds one which is active and pulling balls
                if (drivers[i].GetComponent<DriversManager>().isPullingBall)
                {
                    if (drivers[i].GetComponent<DriversManager>().Shrink()) // Function to shrink the sutructure
                    {
                        j--;
                        print("After Shrink-CollapseWidth- 0 > 3 -currentWidth " + currentWidth);
                        break;
                    }
                    else
                        return;
                }
            }
            if( j == 0)
            {
                break;
            }
            // Iterate over drivers from right to inside
            for (int i = 8 ; i >= 4; i--)
            {
                // If finds one which is active and pulling balls
                if (drivers[i].GetComponent<DriversManager>().isPullingBall)
                {
                    if (drivers[i].GetComponent<DriversManager>().Shrink()) // Function to shrink the sutructure
                    {
                        j--;
                        print("After Shrink-CollapseWidth- 4 > 8 -currentWidth " + currentWidth);
                        break;
                    }
                    else
                        return;
                }
            }
        }
    }

    // Gets number of ball sequence from pool,
    // assing Driver object for first one.
    public void GetSequenceFromPool (AdditionType type, int desiredAddition, int headIndex, int height)
    {
        int startIndex = 0;
        int count = height;

        GameObject headObject;
        List<GameObject> temp = new List<GameObject>();

        if(type == AdditionType.Row)
        {
            startIndex = height;
            desiredAddition = desiredAddition > maxHeight - height ? maxHeight - height : desiredAddition;
            count += desiredAddition;

            headObject = lastBalls[headIndex];
        }
        else // AdditionType.Column
        {
            headObject = drivers[headIndex];
        }

        for (int i = startIndex ; i < count ; i++)
        {
            GameObject current = GetObjectFromPool();
            if (!current) return;

            temp.Add(current);

            if(temp.IndexOf(current) == 0)  // if current is first ball that is following Driver
            {
                current.GetComponent<Follower>().head = headObject;

                if (type == AdditionType.Column)
                {
                    headObject.GetComponent<DriversManager>().headFollower = current;
                    headObject.GetComponent<DriversManager>().isPullingBall = true;
                }
                // No need to set headFollower for this object if AdditionType is Row. Because they appear behind existing one.
            }
            // Set head for each object. Every ball has to be head of previous one.
            else
            {
                current.GetComponent<Follower>().head = temp[ temp.IndexOf(current) -1 ];
            }

            // Set properties for reactivating in scene
            current.GetComponent<Follower>().isFollowing = true;
            current.transform.SetParent(null);
            current.transform.position = current.GetComponent<Follower>().head.transform.position + new Vector3(0, 0, -0.7f);
            current.SetActive(true);

            // put the last one in the lastBall list. 
            if (i == count - 1)
            {
                lastBalls[headIndex] = current;
            }
        }
    }

    // Get object from pool if deactive
    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }     
        }
        return null;
    }

    // Instantiate balls and make visible only desired number of at start. 100 (5*20) times
    public void InitialSetups()
    {
        // Iterate over columns
        for (int i = 0; i < 9 ; i++)
        {
            // iterate over rows
            for (int j = 0; j < 60; j++)
            {
                //  Instantiate balls
                GameObject go = Instantiate(ball, new Vector3( (i - 4) * 0.7f , 1.7f, -j * 0.7f), Quaternion.identity);

                // Add ever object to pool
                poolList.Add(go);

                if( j == 0) // If any of first ball to follow driver
                {
                    // Set driver as head
                    // Set myself as headFollower
                    go.GetComponent<Follower>().head = drivers[i];
                    drivers[i].GetComponent<DriversManager>().headFollower = go;
                }
                //  Other sequenced balls
                else
                    go.GetComponent<Follower>().head = poolList[poolList.IndexOf(go) - 1];

                // Track which balls at endmost
                if(j == 19)
                    lastBalls.Add(go);

                //  Hide columns optionaly
                switch (i)
                {
                    case 0:
                    case 1:
                    case 7:
                    case 8:
                        go.GetComponent<Follower>().ReturnToPool();
                        break;
                }
                //  Hide rows after line 20
                if (j > 19)
                    go.GetComponent<Follower>().ReturnToPool();
            }
        }
        // Detect new left and right head corners
        driversManager.FindCorners();
    }
}
