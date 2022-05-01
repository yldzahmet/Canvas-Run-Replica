using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    internal static bool startDriving = false;
    PoolController poolController;

    private void Awake()
    {
        poolController = GetComponent<PoolController>();
    }
    public void StartButtonPressed()
    {
        startDriving = true;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        poolController.InitialSetups();
    }
}
