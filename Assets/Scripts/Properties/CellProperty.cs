using UnityEngine;
using GridSystem;
using GameManagement;
namespace MapDesigner
{
    /// <summary>
    /// This script has the properties and functions that are related to Cells.
    /// </summary>
    public class CellProperty : MonoBehaviour
    {
        public Color color;
        public bool isSelected;
        public bool markedForDestruction;
        public MyGrid currentGrid;
        public MyGrid topGrid;
        public Vector2 targetPos;
        public GameObject explosionParticle;

        private GridManager gM;
        private MapDesignerProperties mapProperties;
        private SharedVariables sV;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingOrder = -10;
            
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


        public void DestroyMe()
        {

            #region EXPLOSION SYSTEM (Only instantiate particle if it is necessary.) 
            //Debug.Log("Cell : " + gameObject.name + " is destroyed");
            GameObject[] readyExplosionObjects = GameObject.FindGameObjectsWithTag("explosion");

                bool foundAvailable = false;
                foreach (GameObject item in readyExplosionObjects)
                {
                    ParticleSystem objSystem = item.GetComponent<ParticleSystem>();
                    if (objSystem.isStopped)
                    {
                        Debug.Log("Cell chose already instantiated explosion ");
                        item.transform.position = transform.position;
                        objSystem.Play();
                        foundAvailable = true;
                        break;
                    }
                }
                if (!foundAvailable)
                {
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
               }

            #endregion

            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            int selectedColorIndex = Random.Range(0, mapProperties.colorCount);
            renderer.color = mapProperties.colors[selectedColorIndex];
            color = renderer.color;
            currentGrid.assignedCell = null;
            sV.cellQueue.Add(this, topGrid);
            markedForDestruction = false;
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

    }
}
