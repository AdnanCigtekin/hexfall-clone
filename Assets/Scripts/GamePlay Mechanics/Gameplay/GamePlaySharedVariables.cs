using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
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

        private GamePlayManagement gPM;
        private void Awake()
        {
            gPM = GetComponent<GamePlayManagement>();
        }
    }
}
