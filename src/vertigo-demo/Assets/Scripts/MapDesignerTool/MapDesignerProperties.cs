﻿
using UnityEngine;

namespace MapDesigner
{
    /// <summary>
    /// Property object for Map Designer Tool.
    /// </summary>
    [System.Serializable]
    public class MapDesignerProperties : MonoBehaviour
    {
        public int horizontalAmount = 8;
        public int verticalAmount = 9;
        public int horizontalUpperLimit = 15;
        public int verticalUpperLimit = 15;
        public float tilePadding = 1;
        public GameObject tileObj;
        public Color[] colors = new Color[0];
        public int colorCount = 1;
        public string levelName = "myLevel";
        public GameObject explosionParticle;
        public GameObject borderObject;
        public GameObject gameOverExplosionParticle;
        public Sprite bombSprite;
        public int ScoreToCreateBomb;
        public int BombExplosionTime;

    }
}