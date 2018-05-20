using System;
using System.Text;
using UnityEngine;

public class TextTest : MonoBehaviour
{
    void Start()
    {
        int a = 5;
        int data = 0;
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < a * 2 - 1; i++)
        {
            data = 0;
            for (int j = 0; j < a * 2 - 1; j++)
            {
                if (j >= a) data--;
                else data++;

                if (i < a)
                {
                    if (i >= data) str.Append("*");
                    else str.Append(data);
                    
                }
                else
                {
                    if (a * 2 - i - 2 >= data) str.Append("*");
                    else str.Append(data);
                }
            }
            str.Append("\n");
        }

        Debug.Log(str.ToString());
    }

}
