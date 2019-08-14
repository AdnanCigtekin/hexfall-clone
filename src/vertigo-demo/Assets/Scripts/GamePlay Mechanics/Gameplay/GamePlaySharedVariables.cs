using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
/// <summary>
/// This script is responsible for storing shared variables for gameplay logic of user interactions.
/// </summary>
namespace GamePlay {
    public class GamePlaySharedVariables : MonoBehaviour
    {
        private MyGrid pSelectedGrid = null;
        public MyGrid selectedGrid
        {
            get { return pSelectedGrid; }

            set {pSelectedGrid = value;
                gPM.SelectCell(pSelectedGrid);
            }

        }

        private int pSelectionMoveDirection = 0;
        public int selectionMoveDirection
        {
            get { return pSelectionMoveDirection; }

            set { pSelectionMoveDirection = value;
                gPM.moveDir = selectionMoveDirection; }
        }

        public bool pIsSliding = false;
        public bool isSliding {
            get { return pIsSliding; }

            set
            {
                pIsSliding = value;
                gPM.isSliding = pIsSliding;
            }
        }

        public bool thereIsDestruction;
        
        private GamePlayController gPM;
        private void Awake()
        {
            gPM = GetComponent<GamePlayController>();
        }
    }
}
