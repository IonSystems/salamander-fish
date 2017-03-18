using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

	Player player;

	void Start () {
		player = GetComponent<Player> ();
	}

	void Update () {
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		player.SetDirectionalInput (directionalInput);

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) {
			player.OnJumpInputDown ();
		}
		if (Input.GetKeyUp (KeyCode.Space) || Input.GetKeyUp (KeyCode.W)) {
			player.OnJumpInputUp ();
		}
		if(Input.GetButton("Fire1")) {
			player.OnFireInput();
		}
		if(Input.GetButton("Pause")){
			if(Time.timeScale == 1) Time.timeScale = 0;
			if(Time.timeScale == 0) Time.timeScale = 1;
		}
	}
}
