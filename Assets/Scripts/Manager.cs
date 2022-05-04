using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    internal static bool startDriving = false;
    PoolController poolController;

    public Text scoreText;
    public string scoreStr = "Score: ";
    public static int score = 0;

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

    private void Update()
    {
        if(scoreText)
            scoreText.text = scoreStr + score.ToString();
    }
}
