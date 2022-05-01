using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ExpandBalls : MonoBehaviour
{
    public PoolController poolController;
    public Text expanderText;
    public string Width = "Width", Height = "Height";
    internal int number = 0;
    private int showedNumber;
    // Start is called before the first frame update


    private void Start()
    {
        number = GenerateNumber();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Controller")
        {
            //poolController.CollapseHeight(number);
            GenerateChanges(number);
        }
    }


    public int GenerateNumber()
    {
        int number = 0;
        if(name == Width)
        {
            do
            {
                number = Random.Range(-8, 9);
                //print("width number: " + number + "\nshowed number: " + showedNumber);
            } while (number == 0);
            
        }
        else
        {
            do
            {
                number = Random.Range(-20, 21);
                showedNumber = number * PoolController.currentWidth;
               // print("height number: " + number + "\nshowed number: " + showedNumber);
            } while (number == 0);
        }
        return number;
    }

    public void GenerateChanges(int number)
    {
        if( number < 0) // Collaplse Calls
        {
            if(name == Width)
            {
                poolController.CollapseWidth(-number);
            }
            else
            {
                poolController.CollapseHeight(-number);
            }
        }
        else // Expand Calls
        {
            if (name == Width)
            {
                poolController.ExpandWidth(number);
            }
            else
            {
                poolController.ExpandHeight(number);

            }
        }
    }
    private void Update()
    {
        if(name == Width)
            showedNumber = number * PoolController.currentHeight;
        else
            showedNumber = number * PoolController.currentWidth;

        expanderText.text = string.Concat(name, "\n", number.ToString());
    }
}
