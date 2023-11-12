/*Zombie.cs could be responsible for everything that makes a zombie unique
 - like managing its health, animations, sound effects, and spawning behavior, health...
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    private MazeCell currentCell;
    private MazeCell targetCell;
    //private enemymove enemyMovement;

    //private void Awake()
    //{
    //    enemyMovement = GetComponent<enemymove>();
    //}



    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        currentCell.OnPlayerEntered();
        targetCell = null;
    }
}