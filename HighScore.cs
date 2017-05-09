using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text[] highscores = new Text[1];
    public int[] highest; //array int for convert from int to string
    
    void Start()
    {        
        
    }

    public int[] ReadScore()  
    {
        using (StreamReader reader = File.OpenText("Records.txt"))
        {
            List<int> score = new List<int>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                score.Add(Convert.ToInt32(line));
            }
            int[] top3results = score.ToArray();
            Array.Sort(top3results, 0, top3results.Length);
            int[] arraytmp = new int[1];
            arraytmp[0] = top3results[top3results.Length - 1];

            return arraytmp;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        highest = new int[1]; 
        highest = ReadScore(); //check and write new top 1 to array

        for (int i = 0; i < highest.Length; i++)
        {
            highscores[0].text = highest[i].ToString();
        }
    }
}
