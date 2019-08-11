using UnityEngine;
using ObjectProperties;
using MapDesigner;
using System.Collections;
using System.Collections.Generic;

namespace GamePlay
{
    public class PossibleMovesChecker : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            cells = GameObject.FindObjectsOfType<CellProperty>();
            CheckPossibleMoves();
        }
        private CellProperty[] cells;
        private float cellPadding;
        // Update is called once per frame
        void Update()
        {

        }


        void CheckPossibleMoves()
        {
            cellPadding = GameObject.FindObjectOfType<MapDesignerProperties>().tilePadding;

            foreach (CellProperty item in cells)
            {
                List<CellProperty> adjacentCells = FindAdjacentCells(cellPadding, item.transform);
                Debug.Log("group for : " + item.name + "    " + string.Join("",
                                                                            new List<CellProperty>(adjacentCells)
                                                                            .ConvertAll(i => i.name)
                                                                             .ToArray()));
                CheckGroup(adjacentCells,item);
            }
            DestroyMarkedCells();
        }

        private void CheckGroup(List<CellProperty> group, CellProperty selectedCell)
        {
            int groupSize = group.ToArray().Length;

            for (int i = 0; i < groupSize; i++)
            {
                for (int j = 0; j < groupSize; j++)
                {
                    if (i == j)
                        continue;
                    float distBetween = Vector2.Distance(group[i].transform.position, group[j].transform.position);
                    if (distBetween <= cellPadding)
                    {
                        CellProperty[] groupToPass = { group[i], group[j], selectedCell };
                        bool markedForDestruction = CheckSameColor(groupToPass);
                        group[i].markedForDestruction = (!group[i].markedForDestruction) ?  markedForDestruction : true;
                        group[j].markedForDestruction = (!group[j].markedForDestruction) ? markedForDestruction : true;
                        selectedCell.markedForDestruction = (!selectedCell.markedForDestruction) ? markedForDestruction : true;
                    }
                }
            }
            
        }

        private void DestroyMarkedCells()
        {
            int cellsSize = cells.Length;
            for (int i = 0; i < cellsSize; i++)
            {
                if(cells[i].markedForDestruction)
                    cells[i].DestroyMe();
            }
        }

        private bool CheckSameColor(CellProperty[] group)
        {
            bool result = true;
            Color defaultColor = group[0].color;
            foreach (CellProperty item in group)
            {
                if (!item.color.Equals(defaultColor))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }


        private List<CellProperty> FindAdjacentCells(float cellPadding, Transform selectedCell)
        {
            List<CellProperty> returnVal = new List<CellProperty>();

            //TODO : Optimize this loop
            foreach (CellProperty item in cells)
            {
                float tempDist = Vector2.Distance(selectedCell.position, item.transform.position);
                if (tempDist <= cellPadding && tempDist != 0)
                {
                    returnVal.Add(item);
                }
            }

            return returnVal;
        }
    }
}