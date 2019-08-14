using MapDesigner;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// This code is responsible for storing utility functions which is needed for checking pairs, checking wheter we have possible moves or not...etc.
    /// </summary>
    public class GridUtil : MonoBehaviour
    {
        public List<MyGrid> FindAdjacentGrids(Transform selectedGrid, MapDesigner.MapDesignerProperties myProperties, MyGrid[] grids)
        {
            List<MyGrid> resultSet = new List<MyGrid>();

            float cellPadding = myProperties.tilePadding;

            foreach (MyGrid item in grids)
            {
                float tempDist = Vector2.Distance(selectedGrid.position, item.transform.position);
                if (tempDist <= cellPadding && tempDist != 0)
                {
                    resultSet.Add(item);
                }
            }

            return resultSet;
        }

        public bool CheckSameColor(MyGrid[] group)
        {
            bool result = true;
            Color defaultColor = group[0].assignedCell.color;
            foreach (MyGrid item in group)
            {
                if (item.assignedCell == null)
                {
                    continue;

                }
                if (!item.assignedCell.color.Equals(defaultColor))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }





        public bool CheckGivenPairForPossibleMoves(MyGrid[] group, MyGrid selectedGrid, MapDesigner.MapDesignerProperties myProperties,MyGrid[] allMap)
        {
           
            float cellPadding = myProperties.tilePadding;


            int groupSize = group.Length;
            for (int i = 0; i < groupSize; i++)
            {
                for (int j = 0; j < groupSize; j++)
                {
                    if (i == j)
                        continue;
                    float distBetween = Vector2.Distance(group[i].transform.position, group[j].transform.position);
                    if (distBetween <= cellPadding)
                    {
                        MyGrid[] newSelectedCellGroup = { selectedGrid, group[i], group[j] };

                        List<MyGrid> tempAdjacentOfCell = new List<MyGrid>();
                        foreach (MyGrid item in newSelectedCellGroup)
                        {
                            tempAdjacentOfCell.AddRange(FindAdjacentGrids(item.transform, myProperties, allMap));
                        }

                        List<MyGrid> adjacentOfCell = tempAdjacentOfCell.Distinct().ToList();
                        for (int a = 0; a < 2; a++)
                        {
                            for (int x = 0; x < groupSize; x++)
                            {
                                for (int y = 0; y < groupSize; y++)
                                {
                                    if (x == y)
                                        continue;
                                    float distBetweenCells = Vector2.Distance(group[x].transform.position, group[y].transform.position);
                                    if (distBetween <= cellPadding)
                                    {
                                        MyGrid[] groupToPass = { group[x], group[y], newSelectedCellGroup[a] };
                                        bool markedForDestruction = CheckSameColor(groupToPass);
                                        if (markedForDestruction)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            return true;
        }




    }
}
