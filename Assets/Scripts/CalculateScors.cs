using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateScors : MonoBehaviour
{
    public int multipler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.score += multipler;
        }
    }
}
