using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectProperties;
using GridSystem;
public class SharedVariables : MonoBehaviour
{
    public bool willGroupCheck = false;
    public bool didGroupCheck = false;
    public bool onGoingCellDown = false;
    public Dictionary<CellProperty, MyGrid> cellQueue = new Dictionary<CellProperty, MyGrid>();

}
