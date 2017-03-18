using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

	private float nextFire;
	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

    [SerializeField]
    private StatusIndicator statusIndicator;

    public PlayerStats stats;

    void Start() {
		controller = GetComponent<Controller2D> ();
		stats = new PlayerStats ();
        
        //retain armour and health
        int test = stats.getArmourOnLoad();
        stats.health = stats.getHealthOnLoad();
        stats.armour = stats.getArmourOnLoad();
        stats.saveStats();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() {
		myTime = myTime + Time.deltaTime;
		CalculateVelocity ();
		HandleWallSliding ();

		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
        if (Input.GetKeyUp(KeyCode.L))
        {
            stats.health = 100;
            stats.armour = 0;
        }
        /*if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}*/
    }

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			//velocity.y = minJumpVelocity;
		}
	}

	public GameObject projectile;
	public float fireDelta = 0.5F;

    public float bulletOffset = 2f;

	private GameObject newProjectile;
	private float myTime = 0.0F;
	private float shootForce = 1500.0f;

	public void OnFireInput(){
		//Debug.Log ("Fire Button pressed");
		if(myTime > nextFire){
			//Debug.Log ("Firing bullet");
			nextFire = myTime + fireDelta;
            //Debug.Log(Input.mousePosition);
           
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse Pos: " + mousePosition + "player pos: " + transform.position);

            Vector3 direction =  mousePosition - transform.position;
            direction.z = 0;
            direction.Normalize();
            
            newProjectile = Instantiate(projectile, transform.position + direction * bulletOffset, transform.rotation) as GameObject;
            Vector3 force = direction * shootForce;
			//Debug.Log ("Force: " + force);
            
            newProjectile.GetComponent<Rigidbody2D>().AddForce(force);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
             newProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            nextFire = nextFire - myTime;
			myTime = 0.0F;
		}
	}
		

	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Armour"))
        {
            Debug.Log("Player picked up Armour");
			this.stats.armourPickup ();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Exit"))
        {
            Debug.Log("In Exit");

        }
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("Hit Enemy");
			this.stats.injury (10);
			if (this.stats.health <= 0) {
				Time.timeScale =0;
				//Destroy (this.gameObject);
			}

		}
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            stats.saveStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}