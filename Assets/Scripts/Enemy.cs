using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 20;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

	private Vector3 direction;
	private Vector3 velocity;
	private Controller2D controller;
	private Vector2 moveDirection;
	Rigidbody2D rb;

    void Start()
    {
        stats.Init();

		direction = new Vector3(1.0f, 1.0f, 1.0f);
		velocity = new Vector3(1.0f, 1.0f, 1.0f);
		moveDirection = Vector2.left;

		controller = GetComponent<Controller2D> ();
		rb = GetComponent<Rigidbody2D>();

    }


	void OnCollisionEnter2D(Collision2D collision)
	{
		moveDirection *= -1.0f;
		if (collision.gameObject.CompareTag("Bullet"))
		{
			this.stats.curHealth -= this.stats.damage;
			if (this.stats.curHealth <= 0) {
				Debug.Log ("Enemy is dead");
				Destroy (this.gameObject);
			}

		}
	}

	void Update() {
		rb.AddForce(moveDirection *10);
	}
}
