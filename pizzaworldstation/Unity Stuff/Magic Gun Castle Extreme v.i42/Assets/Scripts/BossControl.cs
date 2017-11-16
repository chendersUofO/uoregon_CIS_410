using UnityEngine;
using System.Collections;

public class BossControl : MonoBehaviour
{
	Transform player;               // Reference to the player's position.


	//NavMeshAgent nav; 
	public float speed = 10f;
	public float rotationSpeed = 3f; //speed of turning
	public bool playerInRange = false;
	public bool attacking = false;
	Animator anim;
	Animation curAnim;
	Vector3 playerPos;
	Vector3 thisPos;
	Vector3 dist;
	public float attackDist = 0.5f;
	public float attackRate = 1.5f;
	public float summonRate = 10f;
	public float attackCoolDown;
	public float summonCoolDown;
	public int damage;
	public GameObject slim;
	public int maxSpawns = 3;
	int numSpawns = 0;
	Vector3[] positionArray = new Vector3[11];


	void Start ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		anim = GetComponent<Animator>();
		attackCoolDown = 0;
		summonCoolDown = summonRate;

		positionArray[0] = new Vector3(9.0f,0.0f,6.0f);            
		positionArray[1] = new Vector3(1f,0f,1f);
		positionArray[2] = new Vector3(14.0f,0f,0.4f);
		positionArray[3] = new Vector3(-3.0f,0f,-13.0f);
		positionArray[4] = new Vector3(9.0f,0.0f,6.0f);            
		positionArray[5] = new Vector3(1f,0f,10f);
		positionArray[6] = new Vector3(5.0f,0f,-18f);
		positionArray[7] = new Vector3(-3.0f,0f,-13.0f);
		positionArray[8] = new Vector3(9.0f,0.0f,-6.0f);            
		positionArray[8] = new Vector3(15f,0f,-16f);
		positionArray[10] = new Vector3(-14.0f,0f,5f);


	}
	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Player") 
		{
			//anim.Play ("Attacking");
			attacking = true;


		}
	}


	void Update ()
	{

		if (playerInRange == true && (Vector3.Distance(player.position, transform.position) > attackDist))
		{
			transform.rotation = Quaternion.Slerp (transform.rotation,Quaternion.LookRotation (player.position - transform.position), rotationSpeed * Time.deltaTime);
			transform.position += transform.forward * speed * Time.deltaTime;
			attacking = false;
			anim.SetBool ("isWalking", true);
		} 
		else if(Vector3.Distance(player.position, transform.position) < attackDist)
		{
			
			if (attackCoolDown > 0) {
				attackCoolDown -= Time.deltaTime;
			}
			else 
			{
				Attack ();
				attackCoolDown = attackRate;
			}

			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (player.position - transform.position), rotationSpeed * Time.deltaTime);
		}
		attackCoolDown -= Time.deltaTime;
		summonCoolDown -= Time.deltaTime;
		if (summonCoolDown <= 0)
		{
			Summon ();
			summonCoolDown = summonRate;
		}

	} 
	void Attack()
	{
		player.SendMessage ("ApplyDamage", damage);
		anim.Play ("Attacking");


	}
	void Summon()
	{
		
		while (numSpawns < maxSpawns)
		{
			var spawn = (GameObject)Instantiate (slim);
			spawn.transform.position = positionArray[Random.Range(0,10)];
			numSpawns++;
		
		}
		numSpawns = 0;

	}
}