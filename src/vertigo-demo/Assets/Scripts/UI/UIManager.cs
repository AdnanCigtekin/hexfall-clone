using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script is responsible for UI behaviour.
/// </summary>
namespace myUI
{
    public class UIManager : MonoBehaviour
    {
        public Text ScoreValue;
        public Text TurnValue;

        public void SetTextValue(string myObject, int value)
        {
            GameObject myObj = GameObject.Find(myObject);
            myObj.GetComponent<Text>().text = value.ToString();
        }

        public void RestartGame()
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           
        }


    }
}
