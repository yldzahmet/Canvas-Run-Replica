using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public float rotSpeed = 60;
    public float slideDelta = 0;
    private Vector3 startPoint, endPoint;

    private void Start()
    {
        startPoint = transform.position;
        endPoint = transform.position + new Vector3(0, 0.25f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Manager.score += 10;
        }
    }
    void Update()
    {
        transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Lerp(0, 1, slideDelta));
        slideDelta += 1 * Time.deltaTime;

        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

        if (slideDelta > 1)
        {
            slideDelta = 0;
            Vector3 temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
    }
}
