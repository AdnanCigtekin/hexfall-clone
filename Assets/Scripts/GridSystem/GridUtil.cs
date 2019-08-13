using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
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
    }
}
