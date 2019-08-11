using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void DestroyMe()
        {
            Destroy(gameObject);
        }

    }
}
