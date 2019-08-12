using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectProperties;
using MapDesigner;
namespace GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public List<MyGrid> grids;
        [SerializeField]
        private Transform gridObject;
        private MapDesignerProperties mapProperties;
        public MyGrid GenerateGrid(Vector2 newPos, int id, CellProperty newCell)
        {
            if (id == 0)
            {
                grids = new List<MyGrid>();
            }
            MyGrid newGrid = Instantiate(gridObject, newPos, Quaternion.identity).GetComponent<MyGrid>();
            newGrid.GridId = id;
            newGrid.transform.position = newPos;

            newGrid.assignedCell = newCell;
            newGrid.transform.parent = transform;
            newGrid.belowGrid = GetBelowGrid();
            grids.Add(newGrid);
            return newGrid;
        }




        private MyGrid GetBelowGrid()
        {
            if (mapProperties == null)
                mapProperties = GetComponent<MapDesignerProperties>();

            int amountOfHorizontalCells = mapProperties.horizontalAmount;

            if (grids.Count < amountOfHorizontalCells)
                return null;


            int belowRowStartingPoint = grids.Count - amountOfHorizontalCells;



            return grids[belowRowStartingPoint];
        }

    }
}
