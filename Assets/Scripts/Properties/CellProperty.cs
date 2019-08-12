using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;
namespace ObjectProperties
{
    /// <summary>
    /// Property object for cells in the level
    /// </summary>
    public class CellProperty : MonoBehaviour
    {
        public Color color;
        public bool isSelected;
        public bool markedForDestruction;
        public MyGrid currentGrid;
        public MyGrid topGrid;
        private GridManager gM;
        private MapDesignerProperties mapProperties;
        private SharedVariables sV;
        private void Awake()
        {
            gM = FindObjectOfType<GridManager>();
            mapProperties = FindObjectOfType<MapDesignerProperties>();
            sV = FindObjectOfType<SharedVariables>();
            topGrid = getTopGrid(currentGrid);
        }

        public void DestroyMe()
        {
            Debug.Log("Cell : " + gameObject.name + " is destroyed");
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            int selectedColorIndex = Random.Range(0, mapProperties.colorCount);
            renderer.color = mapProperties.colors[selectedColorIndex];
            color = renderer.color;
            currentGrid.assignedCell = null;
            sV.cellQueue.Add(this, topGrid);
            markedForDestruction = false;
        }

        private MyGrid getTopGrid(MyGrid currentGrid)
        {
            if (mapProperties == null)
                mapProperties = GetComponent<MapDesignerProperties>();

            int gridId = currentGrid.GridId;
            int gridSize = gM.grids.Count;
            int horizontalCellAmount = mapProperties.horizontalAmount;

            int topGridIndex = gridSize - (horizontalCellAmount - (gridId % horizontalCellAmount));
          //  Debug.Log("topGridIndex : " + topGridIndex);
            return gM.grids[topGridIndex];
        }

    }
}
