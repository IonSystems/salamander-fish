using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace PlatformerE13

{

    public class BoardManager : MonoBehaviour
    {
        private static ILogger logger = Debug.logger;
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class Count
        {
            public int minimum;             //Minimum value for our Count class.
            public int maximum;             //Maximum value for our Count class.


            //Assignment constructor.
            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }

        public int columns = 50;                                         
        public int rows = 50;

        public Count platformCount = new Count(1, 1);                      //Lower and upper limit for our random number of walls per level.
        public Count movingPlatformCount = new Count(1, 1);                      //Lower and upper limit for our random number of food items per level.
        public Count armourCount = new Count(1, 1);
        public Count enemyCount = new Count(1, 1);
        public Count doorCount = new Count(1, 1);

        public Vector3 platformScaleMin = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 platformScaleMax = new Vector3(0.1f, 0.1f, 0.0f);

        public GameObject[] platformTiles;
        public GameObject[] movingPlatformTiles;
        public GameObject[] armourTiles;
        public GameObject[] enemyTiles;
        public GameObject[] doorTiles;

        private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
        private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.
        private List<Vector3> platformLocations = new List<Vector3>();

        public GameObject platformSet;

        //Clears our list gridPositions and prepares it to generate a new board.
        void InitialiseList()
        {
            //Clear our list gridPositions.
            gridPositions.Clear();

            //Loop through x axis (columns).
            for (int x = 1; x < columns - (1 + (int)platformScaleMax.x); x++)
            {
                //Within each column, loop through y axis (rows).
                for (int y = 1; y < rows - (1 + (int)platformScaleMax.y); y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }


        //RandomPosition returns a random position from our list gridPositions.
        Vector3 RandomPosition(string tileType)
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range(0, gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            gridPositions.RemoveAt(randomIndex);

            //add entry to platform locations
            if (tileType.Equals("platform"))
            {
                platformLocations.Add(randomPosition);
            }

            //Return the randomly selected Vector3 position.
            return randomPosition;
        }


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum, string tileType="")
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            int objectCount = Random.Range(minimum, maximum + 1);
            logger.Log("object Count: " + objectCount);
            logger.Log("object min: " + minimum);
            logger.Log("object max: " + maximum);
            logger.Log("object platform Loc: " + platformLocations.Count);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (int i = 0; i < objectCount; i++)
            {
                float randx = Random.Range(platformScaleMin.x, platformScaleMax.x);
                float randy = Random.Range(platformScaleMin.y, platformScaleMax.y);
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                Vector3 randomPosition = RandomPosition(tileType);

                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                
                if (tileType.Equals("platform"))
                {
                    
                    tileChoice.transform.localScale = new Vector3(randx,
                                                                  randy,
                                                                  0.0f
                                                                  );

                    GameObject curplatform = Instantiate(tileChoice, randomPosition, Quaternion.identity);
                    Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
                    curplatform.GetComponent<MeshRenderer>().material.color = newColor;

                }
                else if (tileType.Equals("movingP"))
                {
                    tileChoice.transform.localScale = new Vector3(randx,
                                                                  randy,
                                                                  0.0f
                                                                  );
                    Instantiate(tileChoice, randomPosition, Quaternion.identity);
                }
                else if (tileType.Equals("door"))
                {
                    int randomIndex = Random.Range(0, platformLocations.Count);
                    Vector3 doorLocation = platformLocations[randomIndex];
                    Instantiate(tileChoice, doorLocation, Quaternion.identity);

                }else
                {
                    //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                    Instantiate(tileChoice, randomPosition, Quaternion.identity);
                }
                
            }
        }


        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {

            //Reset our list of gridpositions.
            InitialiseList();
            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(platformTiles,         platformCount.minimum, platformCount.maximum, "platform");
            LayoutObjectAtRandom(movingPlatformTiles,   movingPlatformCount.minimum, movingPlatformCount.maximum, "movingP");
            LayoutObjectAtRandom(armourTiles,           armourCount.minimum, armourCount.maximum);
            LayoutObjectAtRandom(doorTiles,             doorCount.minimum, doorCount.maximum, "door");


            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = (int)Mathf.Log(level, 2f) + 5;

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

            //Instantiate the exit tile in the upper right hand corner of our game board
            //Instantiate(doorTiles[1], new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        }
    }
}