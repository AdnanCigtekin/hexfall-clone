using MapDesigner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

namespace GamePlay
{
    /// <summary>
    /// This script is responsible for detecting user input.
    /// </summary>
    [RequireComponent(typeof(GamePlaySharedVariables))]
    public class UserInput : MonoBehaviour
    {
        private Camera cam;
        private Touch finger;

        private GamePlaySharedVariables sH;

        [SerializeField]
        private float slidePrecision = 0.1f;


        private bool canChooseAgain = true;



        private void Start()
        {
            cam = Camera.main;
            sH = GetComponent<GamePlaySharedVariables>();
        }
        private float firstYVal = 0;
        private void Update()
        {
            if (Input.touchCount > 0)
            {

                finger = Input.GetTouch(0);

                Ray ray = Camera.main.ScreenPointToRay(finger.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);



                if (finger.phase == TouchPhase.Began)
                {
                    firstYVal = finger.position.y;
                }
                else if (finger.phase == TouchPhase.Ended || finger.phase == TouchPhase.Canceled)
                {
                    //Debug.Log(finger.position.y - firstYVal);
                    if (finger.position.y - firstYVal > slidePrecision)
                    {
                        Debug.Log("Going counter clockwise");
                        sH.selectionMoveDirection = -1; // counterClockwise
                    }
                    else if (finger.position.y - firstYVal < -slidePrecision)
                    {
                        Debug.Log("Going clockwise");
                        sH.selectionMoveDirection = 1;  // clockwise
                    }
                    else
                    {
                       
                        sH.selectionMoveDirection = 0;
                        if (hit.collider != null)
                        {
                            Debug.Log("Entered 0 for grid id : " + hit.collider.gameObject.GetComponent<CellProperty>().currentGrid.GridId);
                            sH.selectedGrid = hit.collider.gameObject.GetComponent<CellProperty>().currentGrid;
                        }
                    }
                    firstYVal = 0;
                }


            }




            else if (Input.touchCount == 0)
            {
                canChooseAgain = true;

            }


        }


    }
}

