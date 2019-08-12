using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MapDesigner;

namespace GameManagement
{
    /// <summary>
    /// This script controls the flow of the cells.
    /// Responsibilities:
    ///     Deciding which cell will move down.
    ///     Deciding which cell will be put on top of the map.
    ///     Execute the needed operations for these decisions.
    /// </summary>
    public class CellFlow : MonoBehaviour
    {
        private SharedVariables sV;
        private GridManager gM;
        private MapDesignerProperties mapProperties;

        private MyGrid[] grids;
        private float timer = 0;


        // Start is called before the first frame update
        void Awake()
        {
            sV = FindObjectOfType<SharedVariables>();
            gM = FindObjectOfType<GridManager>();
            mapProperties = FindObjectOfType<MapDesignerProperties>();
            grids = gM.grids.ToArray();

            int ColorCount = mapProperties.colorCount;
            Color[] mapColors = mapProperties.colors;

            foreach (MyGrid item in grids)
            {
                int randColor = Random.Range(0, ColorCount);
                item.assignedCell.color = mapColors[randColor];
                item.assignedCell.GetComponent<SpriteRenderer>().color = mapColors[randColor];
            }
        }

        void Update()
        {
            timer += Time.deltaTime;
            if ((sV.didGroupCheck || sV.onGoingCellDown) && timer > 0.25f)
            {
                // Debug.Log("did cell flow");
                GetCellsGoDown();
                sV.didGroupCheck = false;
                CreateNewCellsAtTop();
                timer = 0;
            }
            else if (!sV.didGroupCheck && !sV.onGoingCellDown)
            {
                sV.willGroupCheck = true;
            }

        }

        void CreateNewCellsAtTop()
        {

            List<CellProperty> keys = new List<CellProperty>();
            if (sV.cellQueue.Count != 0)
            {

                foreach (KeyValuePair<CellProperty, MyGrid> item in sV.cellQueue)
                {
                    if (item.Value.assignedCell == null)
                    {
                        item.Key.GetComponent<SpriteRenderer>().enabled = true;
                        item.Key.currentGrid = item.Value;
                        item.Value.assignedCell = item.Key;
                        item.Key.transform.position = item.Value.transform.position;
                        item.Key.targetPos = item.Value.transform.position;
                        item.Key.markedForDestruction = false;
                        keys.Add(item.Key);
                    }
                }

                foreach (CellProperty item in keys)
                {
                    sV.cellQueue.Remove(item);
                }

            }

        }

        void GetCellsGoDown()
        {
            sV.onGoingCellDown = true;
            foreach (MyGrid item in grids)
            {
                if (item.assignedCell != null)
                {
                    item.assignedCell.markedForDestruction = false;
                }
                if (item.assignedCell != null && item.belowGrid != null && item.belowGrid.assignedCell == null)
                {

                    item.belowGrid.assignedCell = item.assignedCell;
                    item.assignedCell.currentGrid = item.belowGrid;
                    //item.assignedCell.transform.position = item.belowGrid.transform.position;
                    item.assignedCell.targetPos = item.belowGrid.transform.position;
                    item.assignedCell = null;
                }

            }
            CheckCellSitutation();
        }

        void CheckCellSitutation()
        {
            bool result = false;
            foreach (MyGrid item in grids)
            {
                if (item.assignedCell == null)
                {
                    result = true;
                    break;
                }
            }
            sV.onGoingCellDown = result;
        }
    }
}
