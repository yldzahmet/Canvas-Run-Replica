using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driver") && other.GetComponent<DriversManager>().isPullingBall)
        {
            other.GetComponent<DriversManager>().Shrink();
            print("Obstacle-currentWidth: " + PoolController.currentWidth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
