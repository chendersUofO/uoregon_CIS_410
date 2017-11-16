using UnityEngine;
using System.Collections;

public class playerDetection : MonoBehaviour {

	EnemyController parentController; 
	Animator parentAnim;

	void Start()
	{
		parentController = this.transform.parent.GetComponent<EnemyController>();
		parentAnim = this.transform.parent.GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			parentController.playerInRange = true;
			parentAnim.SetBool ("isWalking", true);
		}
	}
	// Use this for initialization
}