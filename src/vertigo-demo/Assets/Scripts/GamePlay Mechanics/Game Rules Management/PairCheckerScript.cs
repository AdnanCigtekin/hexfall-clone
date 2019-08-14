using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;
using GamePlay;

namespace GameManagement
{
    /// <summary>
    /// This script is responsible for checking whether the map has matching pairs which needs to be destroyed.
    /// </summary> 
    [RequireComponent(typeof(GridUtil))]
    public class PairCheckerScript : MonoBehaviour
    {
        private MyGrid[] grids;
        private MapDesignerProperties myProperties;
        private SharedVariables sV;
        private GridUtil gridUtil;

        private void Awake()
        {
            grids = FindObjectsOfType<MyGrid>();
            myProperties = FindObjectOfType<MapDesignerProperties>();
            gridUtil = GetComponent<GridUtil>();

            sV = FindObjectOfType<SharedVariables>();

        }
        private float Timer = 0;
        private float maxTimer = 1f;
        private void Update()
        {
            if (sV.willGroupCheck )
            {


               

                CheckAllPairs();

            }
            else if(sV.thereIsDestruction)
            {
                
                foreach (MyGrid grid in grids)
                {
                    if (grid.assignedCell != null && grid.assignedCell.markedForDestruction)
                    {
                        
                        sV.thereIsDestruction = true;
                        break;
                    }
                }
                


            }

            if (!sV.onGoingCellDown)
            {
                if (Timer > maxTimer)
                {
                    bool dontCheck = false;


                    foreach (MyGrid item in grids)
                    {

                        if (item.assignedCell == null)
                        {
                            dontCheck = true;
                        }

                    }
                    if (!dontCheck)
                    {
                        bool isGameOver = false;
                        foreach (MyGrid item in grids)
                        {
                            if (item.assignedCell == null)
                            {
                                isGameOver = false;
                                break;
                            }
                            isGameOver = gridUtil.CheckGivenPairForPossibleMoves(gridUtil.FindAdjacentGrids(item.transform, myProperties, grids).ToArray(), item, myProperties, grids);
                            
                            if (!isGameOver)
                            {
                                break;
                            }
                        }
                        sV.gameOver = isGameOver;

                    }
                    Timer = 0;
                }
                Timer += Time.deltaTime;
            }
            else
            {
                Timer = 0;
            }

        }

        public void CheckAllPairs()
        {
            sV.willGroupCheck = false;



            foreach (MyGrid item in grids)
            {
                List<MyGrid> adjacentGrids =  gridUtil.FindAdjacentGrids(item.transform, myProperties, grids);
                CheckGivenPairForDestruction(adjacentGrids, item);


            }
            DestroyMarkedCells(grids);
            sV.didGroupCheck = true;

        }


        private void CheckGivenPairForDestruction(List<MyGrid> group, MyGrid selectedGrid)
        {
            int groupSize = group.ToArray().Length;
            float cellPadding = myProperties.tilePadding;

            for (int i = 0; i < groupSize; i++)
            {
                for (int j = 0; j < groupSize; j++)
                {
                    if (i == j)
                        continue;
                    float distBetween = Vector2.Distance(group[i].transform.position, group[j].transform.position);
                    if (distBetween <= cellPadding)
                    {
                        MyGrid[] groupToPass = { group[i], group[j], selectedGrid };
                        bool markedForDestruction = gridUtil.CheckSameColor(groupToPass);

                        group[i].assignedCell.markedForDestruction = (!group[i].assignedCell.markedForDestruction) ? markedForDestruction : true;
                        group[j].assignedCell.markedForDestruction = (!group[j].assignedCell.markedForDestruction) ? markedForDestruction : true;
                        selectedGrid.assignedCell.markedForDestruction = (!selectedGrid.assignedCell.markedForDestruction) ? markedForDestruction : true;
                    }
                }
            }
        }







        public void DestroyMarkedCells(MyGrid[] grids)
        {
            bool thereIsDestruction = false;
            int gridSize = grids.Length;
            for (int i = 0; i < gridSize; i++)
            {
                if (grids[i].assignedCell.markedForDestruction)
                {
                    thereIsDestruction = true;
                    grids[i].assignedCell.DestroyMe();

                }
            }
            if (thereIsDestruction)
            {
                foreach (MyGrid item in grids)
                {
                    if (item.assignedCell != null)
                    {
                        item.assignedCell.isSelected = false;
                        if (item.assignedCell.isBomb)
                        {
                            item.assignedCell.bombRemainingTurn--;
                        }
                    }
                }
            }
            sV.thereIsDestruction = thereIsDestruction;
            if (thereIsDestruction)
            {
                sV.Turn++;
            }
        }


    }
}
