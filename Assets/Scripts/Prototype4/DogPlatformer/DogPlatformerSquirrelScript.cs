using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerSquirrelScript : MonoBehaviour
{

	Rigidbody2D squirrelRB;

	public float MoveRate = 1;
	public float MoveForce;

	private float timer = -1;

    void Start()
    {
        squirrelRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
		if(timer < 0){
			timer += MoveRate;
			squirrelRB.AddForce(Vector2.right * Mathf.Sign(Random.Range(-1f,1f)) * MoveForce, ForceMode2D.Impulse);
		}
    }
}
