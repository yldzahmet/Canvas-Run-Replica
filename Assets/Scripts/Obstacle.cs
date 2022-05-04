using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driver") && other.GetComponent<DriversManager>().isPullingBall)
        {
            other.GetComponent<DriversManager>().Shrink();
            print("Obstacle-currentWidth: " + PoolController.currentWidth);
        }
    }
}
