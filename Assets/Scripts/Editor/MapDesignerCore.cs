using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

namespace MapDesigner
{
    /// <summary>
    /// Main code for level design tool.
    /// </summary>
    public class MapDesignerCore : EditorWindow
    {

        #region TEMPORARY VARIABLES

        //Used for checking previous values of properties to auto-generate level in real-time
            private int prevHorizontalAmount;
            private int prevVerticalAmount;
            private float prevTiling;
        // //////////////////////////////////////////////////////////////////////////////////

        //Flag for checking whether the tool will update the map or not
        private bool willUpdate;

        // Queue for storing temporary cells for preview.
        private Queue<GameObject> objList = new Queue<GameObject>();

        //Path to properties JSON file.
        private const string propertiesPath =  "/Scripts/Editor/Resources/map-design-properties.json";

        //GameObject for storing previously created map.
        private GameObject oldLevel = null;

        #endregion

        //Used for easily accessing, saving and loading the properties of the map
        private MapDesignerProperties myProperties;


        [MenuItem("Window/Map Designer")]
        static void Init()
        {
            MapDesignerCore window = (MapDesignerCore)GetWindow(typeof(MapDesignerCore));
            window.Show();
        }

        public void Awake()
        {
            ReadDefaults();
            willUpdate = true;

            GameObject tempOldLevel = GameObject.Find(myProperties.levelName);
            if (tempOldLevel != null)
            {
                oldLevel = tempOldLevel;
            }
        }

        // Read the JSON file from the "propertiesPath".
        // If the wanted file couldn't be found in the given directory, generate one.
        // TODO: Add capabilities for checking whether the directory exists or not.
        void ReadDefaults()
        {
            if (File.Exists(Application.dataPath + propertiesPath))
            {

                string readProperties = File.ReadAllText(Application.dataPath + propertiesPath);
                myProperties = JsonUtility.FromJson<MapDesignerProperties>(readProperties);


            }
            else
            {
                myProperties = new MapDesignerProperties();
                myProperties.horizontalAmount = 8;
                myProperties.verticalAmount = 9;
                myProperties.tilePadding = 1;
                myProperties.horizontalUpperLimit = 15;
                myProperties.verticalUpperLimit = 15;

                File.WriteAllText(Application.dataPath + propertiesPath, JsonUtility.ToJson(myProperties));
                Debug.LogError("Couldn't find the resources file, creating now...");
            }
        }
        
        // Save the JSON file for properties.
        void SaveDefaults()
        {
            File.WriteAllText(Application.dataPath + propertiesPath, JsonUtility.ToJson(myProperties));
        }

        // Generate the real level object which will be played by the player.
        void GenerateLevel()
        {
            DestroyImmediate(oldLevel);

            if (objList.ToArray().Length != 0)
            {
                Transform newParent = new GameObject().transform;
                newParent.name = myProperties.levelName;
                int i = 0;
                foreach (GameObject item in objList)
                {
                    GameObject newObj = Instantiate(item, item.transform.position, Quaternion.identity);
                    newObj.transform.parent = newParent;
                    newObj.name = "Cell-" + i;
                    newObj.AddComponent<ObjectProperties.CellProperty>();
                    newObj.GetComponent<ObjectProperties.CellProperty>().color = item.GetComponent<SpriteRenderer>().color;
                    i++;
                }
            }
        }

        //Clean & Save everything
        private void OnDestroy()
        {
            CleanTemporaryObjects();
            SaveDefaults();
        }

        // Destroy the preview cells
        private void CleanTemporaryObjects()
        {
            //Cleaning process for objList
            GameObject tempObj;
            while (objList.ToArray().Length != 0)
            {
                tempObj = objList.Dequeue();

                DestroyImmediate(tempObj);

            }
        }

        //Main GUI flow
        // TODO: Add field and mechanism for customizing created level's name.
        void OnGUI()
        {
            if (myProperties == null)
                return;
            EqualizeDimensions();

            GUILayout.Label("Define the map size by modifying below sliders", EditorStyles.boldLabel);

            myProperties.horizontalAmount = EditorGUILayout.IntSlider("X", myProperties.horizontalAmount, 0, myProperties.horizontalUpperLimit);
            myProperties.verticalAmount = EditorGUILayout.IntSlider("Y", myProperties.verticalAmount, 0, myProperties.verticalUpperLimit);

            myProperties.tileObj = (GameObject)EditorGUILayout.ObjectField(myProperties.tileObj, typeof(GameObject), false);

            myProperties.tilePadding = EditorGUILayout.FloatField("Padding", myProperties.tilePadding);

            myProperties.colorCount = EditorGUILayout.IntField("Color Amount", myProperties.colorCount);

            if(myProperties.colors.Length != myProperties.colorCount)
            {
                Array.Resize(ref myProperties.colors, myProperties.colorCount);
            }


            for (int i = 0; i < myProperties.colorCount; i++)
            {
                myProperties.colors[i] = EditorGUILayout.ColorField(myProperties.colors[i]);
            }

            GUILayout.Label("EDITOR PROPERTIES", EditorStyles.boldLabel);
            GUILayout.Label("Maximum Dimension sizes", EditorStyles.miniBoldLabel);
            myProperties.horizontalUpperLimit = EditorGUILayout.IntField("X", myProperties.horizontalUpperLimit);
            myProperties.verticalUpperLimit = EditorGUILayout.IntField("Y", myProperties.verticalUpperLimit);
            if (GUILayout.Button("Save"))
            {
                GenerateLevel();
                SaveDefaults();
            }

            CheckChanges();
            ApplyChanges();
        }

        // Create the preview objects according to properties.
        void ApplyChanges()
        {
            if (willUpdate && myProperties.tileObj != null)
            {
                CleanTemporaryObjects();
                GameObject tempObj;
                

              
                //Generating the template of the level
                for (int i = 0; i < myProperties.verticalAmount; i += 1)
                {
                    int indent = 0;
                    for (int j = 0; j < myProperties.horizontalAmount; j++)
                    {
                        Vector2 newPos = new Vector2(j * myProperties.tilePadding * 0.762f, (i * myProperties.tilePadding) - (myProperties.tilePadding * indent)/ 2);
                        tempObj = Instantiate(myProperties.tileObj, newPos, Quaternion.identity);
                        int randColorNum = UnityEngine.Random.Range(0, myProperties.colorCount);
                        tempObj.GetComponent<SpriteRenderer>().color = myProperties.colors[randColorNum];
                        objList.Enqueue(tempObj);
                       
                      
                        indent = (indent == 1) ? indent = 0 : indent = 1;
                     
                    }

                }
                willUpdate = false;
            }
        }

        // Decision function for whether to update the preview object or not.
        void CheckChanges()
        {
            if (prevHorizontalAmount != myProperties.horizontalAmount ||
                prevVerticalAmount != myProperties.verticalAmount ||
                prevTiling != myProperties.tilePadding)
            {
                willUpdate = true;
            }

            if (myProperties.horizontalUpperLimit < prevHorizontalAmount)
            {
                myProperties.horizontalUpperLimit = prevHorizontalAmount;
            }

            if (myProperties.verticalUpperLimit < prevVerticalAmount)
            {
                myProperties.verticalUpperLimit = prevVerticalAmount;
            }

        }

        // Function for eqalizing the previous values with the new ones.
        void EqualizeDimensions()
        {
            if(myProperties != null)
            {
                prevHorizontalAmount = myProperties.horizontalAmount;
                prevVerticalAmount = myProperties.verticalAmount;
                prevTiling = myProperties.tilePadding;
            }


        }

    }
}
