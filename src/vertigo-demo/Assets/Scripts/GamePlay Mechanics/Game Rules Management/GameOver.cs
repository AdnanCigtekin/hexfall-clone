
using UnityEngine;

/// <summary>
/// This script is responsible for doing the gameover sequence when the player is dead.
/// </summary>
namespace GameManagement
{
    public class GameOver : MonoBehaviour
    {

        public bool isGameOver;
        [SerializeField]
        private RectTransform GameOverText;
        [SerializeField]
        private float wantedYPos;
        void Update()
        {
            if (isGameOver)
            {
                Vector2 newGameOverPosition = new Vector2(GameOverText.anchoredPosition.x, wantedYPos);
                if (Mathf.Abs(Mathf.Abs(GameOverText.anchoredPosition.y) - Mathf.Abs(wantedYPos)) < 10)
                {
                    GameOverText.anchoredPosition = newGameOverPosition;
                    if (Time.timeScale < 0.2F)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = Mathf.Lerp(Time.timeScale, 0, Time.deltaTime);

                    }
                }
                else
                {
                    GameOverText.anchoredPosition = Vector2.Lerp(GameOverText.anchoredPosition, newGameOverPosition, Time.deltaTime * 10);

                }
            }

        }

       
    }
}
