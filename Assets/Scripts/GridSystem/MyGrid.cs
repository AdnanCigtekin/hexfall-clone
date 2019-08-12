using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapDesigner;
namespace GridSystem
{
    /// <summary>
    /// This script is for containing properties and simple functions that is needed for grids in grid system.
    /// </summary>
    public class MyGrid : MonoBehaviour
    {
        public int GridId;
        public CellProperty assignedCell;
        public MyGrid belowGrid;
        
        public void DestroyMe()
        {
            DestroyImmediate(gameObject);
        }

       
    }
}
