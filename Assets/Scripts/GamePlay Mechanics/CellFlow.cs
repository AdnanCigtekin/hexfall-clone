using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using ObjectProperties;
public class CellFlow : MonoBehaviour
{
    private SharedVariables sV;
    private GridManager gM;
    private MyGrid[] grids;

    // Start is called before the first frame update
    void Awake()
    {
        sV = GameObject.FindObjectOfType<SharedVariables>();
        gM = GameObject.FindObjectOfType<GridManager>();
        grids = gM.grids.ToArray();
    }
    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((sV.didGroupCheck || sV.onGoingCellDown) && timer > 0.5f)
        {
            Debug.Log("did cell flow");
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
                item.assignedCell.transform.position = item.belowGrid.transform.position;
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
