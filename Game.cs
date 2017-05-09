using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Game : MonoBehaviour
{

    public static int WallHeight = 20;
    public static int WallWidth = 10;
    public GameObject[] objects;
    public static Transform[,] wall = new Transform[WallWidth, WallHeight];
    public Text SumScore;
    public int CurrentScore = 0;
    HighScore highscore = new HighScore();   

    public void Write(string stroka) 
    {
        using (StreamWriter file = new StreamWriter("Records.txt", true))
        {
            file.WriteLine(stroka);
        }     
    }



    void Start()
    {
        SpawnNextBlock();
    }

    public int CurSc()
    {
        return CurrentScore;
    }

    public bool CheckIsAboveGrid(Blocks block)
    {
        for (int x = 0; x < WallWidth; ++x)
        {
            foreach (Transform child in block.transform)
            {
                Vector2 pos = Round(child.position);
                if (pos.y > WallHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Update()
    {      
       if (CurrentScore > highscore.ReadScore()[0]) 
            Write(CurrentScore.ToString());  //writing to file          
    }

    public void UpDateUI()
    {
        SumScore.text = CurrentScore.ToString();
    }

    public void UpDateScore()
    {
        CurrentScore += 100;
        UpDateUI();
    }
    public bool IsFullRow(int y)
    {
        for (int x = 0; x < WallWidth; ++x)
        {
            if (wall[x, y] == null)
            {
                return false;
            }
        }
        UpDateScore();
        return true;
    }

    public void DeleteBlocksAt(int y)
    {
        for (int x = 0; x < WallWidth; ++x)
        {
            Destroy(wall[x, y].gameObject);
            wall[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < WallWidth; ++x)
        {
            if (wall[x, y] != null)
            {
                wall[x, y - 1] = wall[x, y];
                wall[x, y] = null;
                wall[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < WallHeight; ++i)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y = 0; y < WallHeight; ++y)
        {
            if (IsFullRow(y))
            {
                DeleteBlocksAt(y);
                MoveAllRowsDown(y + 1);
                y--;
            }
        }
    }

    public void UpdateWall(Blocks blocks)
    {
        for (int y = 0; y < WallHeight; ++y)
        {
            for (int x = 0; x < WallWidth; ++x)
            {
                if (wall[x, y] != null)
                {
                    if (wall[x, y].parent == blocks.transform)
                    {
                        wall[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform child in blocks.transform)
        {
            Vector2 pos = Round(child.position);
            if (pos.y < WallHeight)
            {
                wall[(int)pos.x, (int)pos.y] = child;
            }
        }
    }

    public Transform GetTranformAtWallPosition(Vector2 pos)
    {
        if (pos.y > WallHeight - 1)
        {
            return null;
        } else
        {
            return wall[(int)pos.x, (int)pos.y];
        }
    }

    public void SpawnNextBlock()
    {
        GameObject NextBlock = Instantiate(objects[UnityEngine.Random.Range(0, objects.Length)], new Vector2(5.0f, 20.0f), Quaternion.identity);
    }

    public bool CheckIsoutofWall(Vector2 pos)  //Метод, проверяющий выход за пределы поля
    {
        return ((int)pos.x >= 0 && (int)pos.x < WallWidth && (int)pos.y >= 0);
    }

    public Vector2 Round(Vector2 vec)  //Метод, округляющий позиции вектора 2 , до целых чисел
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }

    public void GameOver()
    {
        Write(CurrentScore.ToString()); //writing to file
        Application.LoadLevel("GameOver");
    }
}
