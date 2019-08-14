using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;

namespace GamePlay
{
    [RequireComponent(typeof(GroupPicker))]
    public class GamePlayController : MonoBehaviour
    {
        private GroupPicker gP;
        private MyGrid[] selectedGrids;
        public int moveDir = 0;
        private bool Moving = false;

        private MyGrid prevSelectedCell = null;
        public bool isSliding = false;
        private GamePlaySharedVariables gPSV;

        private void Awake()
        {
            Time.timeScale = 1;
        }


        void Start()
        {
            gP = GetComponent<GroupPicker>();
            gPSV = GetComponent<GamePlaySharedVariables>();
        }
        private float moveTimer;
        private readonly float maxTimer = 0.5f;

        private int lastMoveDir;
        private int moveRemaining = 2;
        private bool doingRoll = false;


        // Update is called once per frame
        void Update()
        {
            CheckMovable();
            //  Debug.Log("Sliding : " + isSliding);
            DoSlideRoll();
            DetectMove();

        }

        private void DoSlideRoll()
        {
            if (Moving)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > maxTimer)
                {
                    moveTimer = 0;
                    slidePair();
                    if (moveRemaining == 0)
                    {
                        Moving = false;
                        moveDir = 0;
                        moveRemaining = 3;
                        doingRoll = false;
                    }
                   
                   
                }
            }
        }

        private void CheckMovable()
        {
            if (gPSV.thereIsDestruction)
            {
                doingRoll = false;
                Moving = false;
                moveDir = 0;
                moveRemaining = 3;
                moveTimer = 0;
                return;
            }
        }

        private void DetectMove()
        {
            if (moveDir != 0 && !Moving && moveTimer == 0 && selectedGrids != null && !doingRoll)
            {
                if (moveRemaining > 0)
                {
                    lastMoveDir = moveDir;
                    
                }
                if (!DetectUnselected())
                {
                    //Move Clockwise
                    if (moveDir < 0)
                    {
                        Moving = true;
                        Debug.Log("Moving clockwise: " + selectedGrids[0].assignedCell.name);
                        moveTimer = 0;
                    }
                    //Move Counter-clockwise
                    else
                    {
                        Moving = true;
                        Debug.Log("Moving counter-clockwise: " + selectedGrids[0].assignedCell.name);
                        moveTimer = 0;
                    }
                    slidePair();
                }

            }
        }

        private void slidePair()
        {
            doingRoll = true;
            if (lastMoveDir > 0)
            {
                selectedGrids[0].assignedCell.currentGrid = selectedGrids[1];   //Cell[A]   ->   Grid[2]
                CellProperty temp = selectedGrids[1].assignedCell;              //var temp  =    Cell[B]
                selectedGrids[1].assignedCell = selectedGrids[0].assignedCell;  //Grid[2]   ->   Cell[A]

                temp.currentGrid = selectedGrids[2];                            //temp      ->   Grid[3]
                CellProperty tempTwo = selectedGrids[2].assignedCell;           //var temp2 =    Cell[C]
                selectedGrids[2].assignedCell = temp;                           //Grid[3]   ->   temp

                tempTwo.currentGrid = selectedGrids[0];
                selectedGrids[0].assignedCell = tempTwo;
            }
            else if(lastMoveDir < 0)
            {
                Debug.Log("going counterclockwise");

                selectedGrids[0].assignedCell.currentGrid = selectedGrids[2];   //Cell[A]   ->   Grid[2]
                CellProperty temp = selectedGrids[2].assignedCell;              //var temp  =    Cell[B]
                selectedGrids[2].assignedCell = selectedGrids[0].assignedCell;  //Grid[2]   ->   Cell[A]

                temp.currentGrid = selectedGrids[1];                            //temp      ->   Grid[3]
                CellProperty tempTwo = selectedGrids[1].assignedCell;           //var temp2 =    Cell[C]
                selectedGrids[1].assignedCell = temp;                           //Grid[3]   ->   temp

                tempTwo.currentGrid = selectedGrids[0];
                selectedGrids[0].assignedCell = tempTwo;
            }

            moveRemaining--;
            gPSV.isSliding = false;


        }
        //return false if there is no cell seems unselected and actually unselected
        //return true  if there is no cell seems unselected but actually selected
        //
        private bool DetectUnselected()
        {
            foreach (MyGrid item in selectedGrids)
            {
                if (!item.assignedCell.isSelected)
                {
                    return true;
                }
            }
            return false;
        }

        public void SelectCell(MyGrid selectedCell)
        {

            if (selectedCell == null|| doingRoll)
            {
                return;
            }


                if (selectedCell.Equals(prevSelectedCell))
                {
                    //Moving = false;
                    selectedCell = prevSelectedCell;
                if (isSliding)
                {
                    return;

                }

                }
                prevSelectedCell = selectedCell;
                if (selectedGrids != null && selectedGrids.Length != 0)
                {
                    foreach (MyGrid item in selectedGrids)
                    {
                        item.assignedCell.isSelected = false;
                    }

                }

                Debug.Log("selected Cell " + selectedCell.assignedCell.name);
                selectedGrids = gP.SelectUserGroup(selectedCell);
                foreach (MyGrid item in selectedGrids)
                {
                    item.assignedCell.isSelected = true;
                }
            }
            

        
    }
}
