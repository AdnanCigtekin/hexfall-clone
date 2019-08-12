using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;

public class PairCheckerScript : MonoBehaviour
{
    private MyGrid[] grids;
    private MapDesignerProperties myProperties;
    private SharedVariables sV;

    private void Awake()
    {
        grids = FindObjectsOfType<MyGrid>();
        myProperties = FindObjectOfType<MapDesignerProperties>();
        sV = FindObjectOfType<SharedVariables>();
        CheckAllPairs();
    }

    private void Update()
    {
        if (sV.willGroupCheck)
        {
            
            CheckAllPairs();
        }
    }

    public void CheckAllPairs()
    {
        sV.willGroupCheck = false;




        foreach (MyGrid item in grids)
        {
            List<MyGrid> adjacentGrids = FindAdjacentGrids(item.transform);
            CheckGivenPair(adjacentGrids, item);
            
        }
        DestroyMarkedCells(grids);
        sV.didGroupCheck = true;
    }


    private void CheckGivenPair(List<MyGrid> group, MyGrid selectedGrid)
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
                    bool markedForDestruction = CheckSameColor(groupToPass);

                    group[i].assignedCell.markedForDestruction = (!group[i].assignedCell.markedForDestruction) ? markedForDestruction : true;
                    group[j].assignedCell.markedForDestruction = (!group[j].assignedCell.markedForDestruction) ? markedForDestruction : true;
                    selectedGrid.assignedCell.markedForDestruction = (!selectedGrid.assignedCell.markedForDestruction) ? markedForDestruction : true;
                }
            }
        }
    }

    private bool CheckSameColor(MyGrid[] group)
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


    private List<MyGrid> FindAdjacentGrids(Transform selectedGrid)
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


    public void DestroyMarkedCells(MyGrid[] grids)
    {
        int gridSize = grids.Length;
        for (int i = 0; i < gridSize; i++)
        {
            if (grids[i].assignedCell.markedForDestruction)
            {
                grids[i].assignedCell.DestroyMe();

            }
        }
        
    }


}
