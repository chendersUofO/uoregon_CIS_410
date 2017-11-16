using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
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
	public float attackRate = 10f;
	public float attackCoolDown;
	public int damage;

	void Start ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		anim = GetComponent<Animator>();
		attackCoolDown = 0;
	}
	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Player") 
		{
			//anim.Play ("Attacking");
			attacking = true;
			//attackWait ();

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
//			anim.SetBool ("isWalking", false);
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

	} 
	void Attack()
	{
		player.SendMessage ("ApplyDamage", damage);
		anim.Play ("Attacking");


	}
}