using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	//public float speed;
	//Rigidbody2D rigidBody;
	public int lifespan = 100;

	void Start ()
	{
        //rigidBody = GetComponent<Rigidbody2D> ();
       // rigidBody.AddForce (Vector2.right * speed);
        //rigidbody.velocity = transform.forward * speed;
        //Debug.Log("Bullet");
		//Destroy(this,lifespan);
	}


	void Update()
	{
        //rigidBody.AddForce(Vector2.right * speed;
        lifespan--;
        if(lifespan < 0)
        {
            Destroy(this.gameObject);
        }

       // Debug.Log(rigidbody.velocity);
    }

	void OnCollisionEnter2D(Collision2D _colInfo)
	{
        if (!_colInfo.gameObject.name.Equals("Player"))
        {
            Destroy(this.gameObject);
        }
	    
	}
}