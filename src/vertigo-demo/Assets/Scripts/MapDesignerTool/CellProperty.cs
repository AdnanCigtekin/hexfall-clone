using UnityEngine;
using GridSystem;
using GameManagement;
using UnityEngine.UI;
namespace MapDesigner
{
    /// <summary>
    /// This script has the properties and functions that are related to Cells.
    /// </summary>
    public class CellProperty : MonoBehaviour
    {
        public Color color;
        private bool pIsSelected = false;
        public bool isSelected
        {
            get
            {
                return pIsSelected;
            }
            set
            {
                pIsSelected = value;
                GenerateMarker(pIsSelected);
            }
        }
        public bool markedForDestruction;
        public MyGrid pCurrentGrid;
        public MyGrid currentGrid
        {
            get
            {
                return pCurrentGrid;
            }
            set
            {
                pCurrentGrid = value;
                targetPos = currentGrid.transform.position;
            }
        }

        public MyGrid topGrid;
        public Vector2 targetPos;
        public GameObject explosionParticle;
        public GameObject gameOverExplosion;
        public GameObject borderObject;
        public Text bombAlerter;
        private GridManager gM;
        private MapDesignerProperties mapProperties;
        private SharedVariables sV;
        public bool isBomb = false;
        private int pBombRemainingTurn;
        public int bombRemainingTurn
        {
            get
            {
                return pBombRemainingTurn;
            }
            set
            {
                pBombRemainingTurn = value;
                bombAlerter.text = pBombRemainingTurn.ToString();
                if (pBombRemainingTurn == 0)
                {
                    sV.gameOver = true;
                    Explode("gameOverExplosion", gameOverExplosion);
                }
                if (sV.gameOver)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingOrder = -10;
            borderObject.GetComponent<SpriteRenderer>().enabled = isSelected;
            bombAlerter = transform.Find("Canvas/Counter").GetComponent<Text>();
            targetPos = currentGrid.transform.position;
            gM = FindObjectOfType<GridManager>();
            mapProperties = FindObjectOfType<MapDesignerProperties>();
            sV = FindObjectOfType<SharedVariables>();
            topGrid = GetTopGrid(currentGrid);
        }

        private void Update()
        {
            float distToTargetPos = Vector2.Distance(transform.position,targetPos);

            if (distToTargetPos < 0.1f)
            {
                transform.position = targetPos;
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, targetPos, 10 * Time.deltaTime);
            }
        }

        private void GenerateMarker(bool toggle)
        {
            if (toggle)
            {
                borderObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                borderObject.GetComponent<SpriteRenderer>().enabled = false;

            }
        }

        /// <summary>
        /// Only instantiate a new explosion particle if necessary
        /// </summary>
        /// <param name="ExplosionTag"></param>
        /// <param name="explosionPrefab"></param>
        private void Explode(string ExplosionTag, GameObject explosionPrefab)
        {
            GameObject[] readyExplosionObjects = GameObject.FindGameObjectsWithTag(ExplosionTag);

            bool foundAvailable = false;
            foreach (GameObject item in readyExplosionObjects)
            {
                ParticleSystem objSystem = item.GetComponent<ParticleSystem>();
                if (objSystem.isStopped)
                {
                    Debug.Log("Cell chose already instantiated explosion ");
                    item.transform.position = transform.position;
                    objSystem.Play();
                    objSystem.GetComponent<AudioSource>().Play();
                    foundAvailable = true;
                    break;
                }
            }
            if (!foundAvailable)
            {
                GameObject newParticle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                newParticle.GetComponent<AudioSource>().Play();
            }
        }

        public void DestroyMe()
        {
            SharedVariables sH = FindObjectOfType<SharedVariables>();
            if (sH.gameOver)
            {
                return;
            }
            bombAlerter.text = "";
            #region if it was a bomb do this
            if (isBomb)
            {
                sH.currentBombs = null;
                isBomb = false;
            }

            #endregion

            #region Check for whether it will be bomb or not

            if (sH.Score - sH.lastScoreBombCreated >= mapProperties.ScoreToCreateBomb)
            {
                sH.lastScoreBombCreated = sH.Score;
                sH.currentBombs = this;
                isBomb = true;
                bombRemainingTurn = mapProperties.BombExplosionTime;
                GetComponent<SpriteRenderer>().sprite = mapProperties.bombSprite;
            }

            #endregion





            Explode("explosion", explosionParticle);



            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            sV.currentLayerOrder--;
            renderer.sortingOrder = sV.currentLayerOrder;

            int selectedColorIndex = Random.Range(0, mapProperties.colorCount);
            renderer.color = mapProperties.colors[selectedColorIndex];

            bombAlerter.enabled = false;
           

            color = renderer.color;
            isSelected = false;
            currentGrid.assignedCell = null;
            sV.cellQueue.Add(this, topGrid);
            markedForDestruction = false;
            sH.Score += 5;
        }

        private MyGrid GetTopGrid(MyGrid currentGrid)
        {
            if (mapProperties == null)
                mapProperties = GetComponent<MapDesignerProperties>();

            int gridId = currentGrid.GridId;
            int gridSize = gM.grids.Count;
            int horizontalCellAmount = mapProperties.horizontalAmount;

            int topGridIndex = gridSize - (horizontalCellAmount - (gridId % horizontalCellAmount));
          //  Debug.Log("topGridIndex : " + topGridIndex);
            return gM.grids[topGridIndex];
        }

        public MyGrid GetAnyTopGrid()
        {
            if (mapProperties == null)
                mapProperties = GetComponent<MapDesignerProperties>();
            int horizontalCellAmount = mapProperties.horizontalAmount;
            int gridSize = gM.grids.Count;

            for (int i = gridSize - horizontalCellAmount - 1; i < gridSize; i++)
            {
               
                if(gM.grids[i].assignedCell == null)
                {
                    return gM.grids[i];
                }
            }
            return null;

        }

    }
}
