using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

namespace GamePlay
{
    [RequireComponent(typeof(GridUtil))]
    public class GroupPicker : MonoBehaviour
    {
        private GridUtil gridUtil;
        private GridManager gridManager;

        private void Awake()
        {
            gridUtil = GetComponent<GridUtil>();
            gridManager = GetComponent<GridManager>();
        }

        public MyGrid[] SelectUserGroup(MyGrid selectedGrid)
        {
            List<MyGrid> adjacentGrids = gridUtil.FindAdjacentGrids(selectedGrid.transform, gridManager.mapProperties, gridManager.grids.ToArray());

            int adjacentGridsCount = adjacentGrids.Count;
            float cellPadding = gridManager.mapProperties.tilePadding;

            Dictionary<int, MyGrid[]> dict = new Dictionary<int, MyGrid[]>();

            int dictionaryKey = 0;
            for (int i = 0; i < adjacentGridsCount; i++)
            {
                for (int j = 0; j < adjacentGridsCount; j++)
                {
                    if (i == j)
                        continue;
                    float distBetween = Vector2.Distance(adjacentGrids[i].transform.position, adjacentGrids[j].transform.position);
                    if (distBetween < cellPadding)
                    {
                        MyGrid[] newPair = { adjacentGrids[i], adjacentGrids[j], selectedGrid };
                        dict.Add(dictionaryKey, newPair);
                        dictionaryKey++;
                    }
                }
            }

            int selectedDictKey = Random.Range(0, dictionaryKey - 1);

            return dict[selectedDictKey];

        }
    }
}
