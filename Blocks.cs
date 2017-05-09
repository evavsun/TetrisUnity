using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public float Fall = 0;  
    public float FallSpeed = 1;

    public bool AllowRotation = true; //variable which allow the rotation of blocks 
    public bool LimitRotation = false; //variable for BlockZ, BlockS, BlockI. BlockO don't have any Rotation

    

    void Start()
    {

    }

    void Update()
    {
        CheckUserInput();
        if(FindObjectOfType<Game>().CurSc()>=1500)
        {
          FallSpeed =0.8f;
        }
        if (FindObjectOfType<Game>().CurSc() > 3200)
        {
            FallSpeed = 0.6f;
        }
        if (FindObjectOfType<Game>().CurSc() > 4500)
        {
            FallSpeed = 0.5f;
        }

    }
    public void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (CheckIsValidPosition())  
            {
                FindObjectOfType<Game>().UpdateWall(this); //Updating all field of game
            } else
            {
                transform.position += new Vector3(-1, 0, 0); //Go one position to the left
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateWall(this);
            } else
            {
                transform.position += new Vector3(1, 0, 0); //Go one position to the right
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (AllowRotation)
            {
                if (LimitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90) //if object has been rotated
                    {
                        transform.Rotate(0, 0, -90);
                    } else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                } else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateWall(this);
            } else
            {
                if (LimitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    } else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                } else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
        } else if (Input.GetKey(KeyCode.DownArrow) || Time.time - Fall >= FallSpeed)
        {
            Thread.Sleep(50);
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateWall(this);
            } else
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }
                enabled = false;
                FindObjectOfType<Game>().SpawnNextBlock();
            }
            Fall = Time.time;
        }
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(child.position);
            if (FindObjectOfType<Game>().CheckIsoutofWall(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTranformAtWallPosition(pos) != null && FindObjectOfType<Game>().GetTranformAtWallPosition(pos).parent!= transform)       
            {               
                return false;         
            }
        }
        return true;
    }
}
