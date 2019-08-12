using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectProperties;
namespace GridSystem
{
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
