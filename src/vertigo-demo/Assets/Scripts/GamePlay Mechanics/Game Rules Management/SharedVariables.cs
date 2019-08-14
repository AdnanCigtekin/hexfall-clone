using System.Collections.Generic;
using UnityEngine;
using MapDesigner;
using GridSystem;
using myUI;
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
        private GameOver gameOverObj;
        private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                UImanager.SetTextValue("Score-val",score);
            }
        }
        private void Awake()
        {
            gPSV = GetComponent<GamePlay.GamePlaySharedVariables>();
            gameOverObj = GetComponent<GameOver>();
            UImanager = FindObjectOfType<UIManager>();
            Application.targetFrameRate = 60;
        }

        public CellProperty currentBombs;
        public int lastScoreBombCreated;
        private bool pGameOver;
        public bool gameOver {
            get {
                return pGameOver;
            }
            set
            {
                pGameOver = value;
                if (pGameOver)
                {
                    gameOverObj.isGameOver = true;
                    Debug.Log("Game Over");
                }
            }
        }
        private UIManager UImanager;
        private int turn;
        public int Turn {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
                UImanager.SetTextValue("Turn-value", turn);
            }
        }
        public int currentLayerOrder = -10;
    }
}