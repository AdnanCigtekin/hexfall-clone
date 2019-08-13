﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;

namespace GamePlay
{
    [RequireComponent(typeof(GroupPicker))]
    public class GamePlayManagement : MonoBehaviour
    {
        private GroupPicker gP;
        private MyGrid[] selectedGrids;
        public int moveDir = 0;
        private bool Moving = false;

        private MyGrid prevSelectedCell = null;
        public bool isSliding = false;
        private GamePlaySharedVariables gPSV;
        // Start is called before the first frame update
        void Start()
        {
            gP = GetComponent<GroupPicker>();
            gPSV = GetComponent<GamePlaySharedVariables>();
        }
        private float moveTimer;
        private float maxTimer = 0.5f;

        private int lastMoveDir;
        private int moveRemaining = 2;
        // Update is called once per frame
        void Update()
        {
            if (gPSV.thereIsDestruction)
            {
                Moving = false;
                moveDir = 0;
                moveRemaining = 2;
                moveTimer = 0;
                return;
            }
          //  Debug.Log("Sliding : " + isSliding);
            if (Moving)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > maxTimer)
                {
                    moveTimer = 0;
                    if (moveRemaining == 0)
                    {
                        Moving = false;
                        moveDir = 0;
                        moveRemaining = 3;
                    }
                    moveRemaining--;
                    slidePair();
                }
            }
            if (moveDir != 0 && !Moving && moveTimer == 0 && selectedGrids != null)
            {
                if (moveRemaining > 0)
                {
                    lastMoveDir = moveDir;
                }
                
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

        private void slidePair()
        {
            if (moveDir > 0)
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
            else if(moveDir < 0)
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


            gPSV.isSliding = false;

            //if (moveDir < 0)
            //{

            //    for (int i = 0; i < selectedGridLength; i++)
            //    {
            //        int index = i +1;
            //        if (index == selectedGridLength)
            //        {
            //            index = 0;
            //        }
            //        selectedGrids[i].assignedCell.currentGrid = tempGrids[index];
            //        selectedGrids[index].assignedCell = tempGrids[i].assignedCell;
            //      //  selectedGrids[index].assignedCell = tempGrids[i].assignedCell;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < selectedGridLength; i++)
            //    {
            //        int index = i + 1;
            //        if (index == selectedGridLength)
            //        {
            //            index = 0;
            //        }
            //        selectedGrids[i].assignedCell.currentGrid = tempGrids[index];
            //        selectedGrids[index].assignedCell = tempGrids[i].assignedCell;
            //    }
            //}

        }


        public void SelectCell(MyGrid selectedCell)
        {

            if (selectedCell == null)
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
