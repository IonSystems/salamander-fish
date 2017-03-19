using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PlatformerE13 {
	public class StartButton : MonoBehaviour {
		
		public Canvas canvas;
		
		public Button start_button;
		public GameObject gameManager; 
		
		void Start () {
			canvas.gameObject.SetActive(true);
			Button btn = start_button.GetComponent<Button>();
			btn.onClick.AddListener(TaskOnClick);
		}
	
		void TaskOnClick(){
			Debug.Log ("You have clicked the button!");
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
	        if (GameManager.instance == null){
				//Instantiate gameManager prefab
				Instantiate(gameManager);
			}
			canvas.gameObject.SetActive(false);
		}
	}
}