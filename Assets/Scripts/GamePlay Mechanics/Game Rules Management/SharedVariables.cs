using System.Collections.Generic;
using UnityEngine;
using MapDesigner;
using GridSystem;

namespace GameManagement
{
    /// <summary>
    /// This script contains the needed variables for gameplay elements.
    /// The need for this file has risen because of instead of having too many references for objects, we can have only one reference for it.
    /// Simplifies the development, on the other hand causes little performance cost because of setting variables will take more CPU cycles.
    /// </summary>
    public class SharedVariables : MonoBehaviour
    {
        public bool willGroupCheck = false;
        public bool didGroupCheck = false;
        public bool onGoingCellDown = false;
        public Dictionary<CellProperty, MyGrid> cellQueue = new Dictionary<CellProperty, MyGrid>();
        private bool pThereIsDestruction;
        public bool thereIsDestruction {
            get
            {
                return pThereIsDestruction;
            }
            set
            {
                pThereIsDestruction = value;
                gPSV.thereIsDestruction = pThereIsDestruction;
            }
        }
        private GamePlay.GamePlaySharedVariables gPSV;
        public int score;
        private void Awake()
        {
            gPSV = GetComponent<GamePlay.GamePlaySharedVariables>();
        }

    }
}