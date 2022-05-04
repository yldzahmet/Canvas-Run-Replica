using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriversManager : MonoBehaviour
{
    PoolController poolController;

    [SerializeField]
    internal bool isPullingBall = true;
    [SerializeField]
    internal GameObject headFollower;
    internal enum Head {None, LeftHead, RightHead};
    [SerializeField]
    internal Head headProperty;

    internal Vector3[] driversPositions = { 
        new Vector3(-2.8f, 0, 0 ),
        new Vector3(-2.1f, 0, 0 ),
        new Vector3(-1.4f, 0, 0 ),
        new Vector3(-0.7f, 0, 0 ),
        new Vector3(0f, 0, 0 ),
        new Vector3(0.7f, 0, 0 ),
        new Vector3(1.4f, 0, 0 ),
        new Vector3(2.1f, 0, 0 ),
        new Vector3(2.8f, 0, 0 )
        };

    void Awake()
    {
        poolController = transform.root.GetComponent<PoolController>();
    }

    // Collapse the structure along the width
    public bool Shrink()
    {
        if (PoolController.currentWidth < 2)
            return false;

        PoolController.currentWidth -= 1;
        int currentIndex = poolController.drivers.IndexOf(gameObject);
        // hides all line behind that driver
        poolController.drivers[currentIndex].GetComponent<DriversManager>().headFollower.GetComponent<Follower>().ReturnToPool();

        // array swipes
        if (currentIndex < 4)
        {
            for (; currentIndex > 0; currentIndex--)
            {

                GameObject tempDriver = poolController.drivers[currentIndex];
                poolController.drivers[currentIndex] = poolController.drivers[currentIndex - 1];
                poolController.drivers[currentIndex - 1] = tempDriver;

                GameObject tempLastBall = poolController.lastBalls[currentIndex - 1];
                poolController.lastBalls[currentIndex - 1] = poolController.lastBalls[currentIndex];
                poolController.lastBalls[currentIndex] = tempLastBall;
            }

        }
        else
        {
            for (; currentIndex < 8; currentIndex++)
            {

                GameObject tempDriver = poolController.drivers[currentIndex];
                poolController.drivers[currentIndex] = poolController.drivers[currentIndex + 1];
                poolController.drivers[currentIndex + 1] = tempDriver;

                GameObject tempLastBall = poolController.lastBalls[currentIndex + 1];
                poolController.lastBalls[currentIndex + 1] = poolController.lastBalls[currentIndex];
                poolController.lastBalls[currentIndex] = tempLastBall;
            }
        }

        // change transforms to current positions
        StartCoroutine(ShrinkCoroutine());
        return true;
    }

    IEnumerator ShrinkCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        
        for (int i = 0; i < PoolController.driverCount ; i++)
        {
            poolController.drivers[i].transform.localPosition = driversPositions[i];
        }

        FindCorners();
    }

    public void FindCorners()
    {
        for (int i = 3; i >= 0; i--)
        {
            if (!poolController.drivers[i].GetComponent<DriversManager>().isPullingBall)
            {
                poolController.drivers[i].GetComponent<DriversManager>().headProperty = Head.LeftHead;
                break;
            }
            poolController.drivers[i].GetComponent<DriversManager>().headProperty = Head.None;

        }
        for (int i = 4 ; i < 9 ; i++)
        {
            if (!poolController.drivers[i].GetComponent<DriversManager>().isPullingBall)
            {
                poolController.drivers[i].GetComponent<DriversManager>().headProperty = Head.RightHead;
                break;
            }
            poolController.drivers[i].GetComponent<DriversManager>().headProperty = Head.None;
        }
    }
}
