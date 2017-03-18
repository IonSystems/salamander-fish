using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUpdate : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Text healthtext = this.gameObject.GetComponent<Text> ();
		//if(healthtext != null){
		Player p = player.GetComponent<Player>();


		healthtext.text = "Health: "+ p.stats.health;
		//}
	}
}
