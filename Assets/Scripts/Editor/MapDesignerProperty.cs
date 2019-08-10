
using UnityEngine;

namespace MapDesigner
{
    /// <summary>
    /// Property object for Map Designer Tool.
    /// </summary>
    public class MapDesignerProperties
    {
        public int horizontalAmount;
        public int verticalAmount;
        public int horizontalUpperLimit;
        public int verticalUpperLimit;
        public float tilePadding;
        public UnityEngine.GameObject tileObj;
        public Color[] colors = new Color[0];
        public int colorCount = 1;
        public string levelName = "myLevel";


    }
}
