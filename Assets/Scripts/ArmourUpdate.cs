using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArmourUpdate : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
        Player p = player.GetComponent<Player>();
        p.stats.armour = p.stats.getArmourOnLoad();
        p.stats.health = p.stats.getHealthOnLoad();
	}
	
	// Update is called once per frame
	void Update () {
		Text armourtext = this.gameObject.GetComponent<Text> ();
		//if(healthtext != null){
		Player p = player.GetComponent<Player>();


		armourtext.text = "Armour: "+ p.stats.armour;
		//}
	}
}
