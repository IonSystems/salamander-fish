using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 


namespace PlatformerE13
{
    public class GameManager : MonoBehaviour
    {
        
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
        public int level = 1;                                  //Current level number, expressed in game as "Day 1".
		public Canvas start_screen;

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<BoardManager>();
			
			start_screen = GetComponent<Canvas>();

            //Call the InitGame function to initialize the first level 
            InitGame(level);
        }

        //Initializes the game for each level.
        public void InitGame(int level)
        {
			start_screen.gameObject.SetActive(true);
			start_screen.enabled = true;
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

        }



        //Update is called every frame.
        void Update()
        {

        }
    }
}