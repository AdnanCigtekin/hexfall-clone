using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

namespace GamePlay
{
    [RequireComponent(typeof(GridUtil))]
    public class GameFlow : MonoBehaviour
    {
        private GridUtil gridUtil;
        // Start is called before the first frame update
        void Start()
        {
            gridUtil = GetComponent<GridUtil>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
