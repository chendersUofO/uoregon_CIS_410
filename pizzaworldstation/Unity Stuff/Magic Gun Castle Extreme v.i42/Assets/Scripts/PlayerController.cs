using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed = 15f;            // The speed that the player will move at.

	Vector3 movement;                   // The vector to store the direction of the player's movement.
	//Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	public static int playerHealth = 100;
	Animator anim;

	public bool usingMagic = true;
	public static bool hasKey = false;
	//public bool usingWeapon;
	public static bool isMagic;  		// current projectile is magic
	public static bool isWeapon;		// current projectile is weapon (gun)
	public static bool isDefault;		// current projectile is default
	public static int ammo = 0; 		// is set to the number of ammo player has
	public static int magicAmmo = 0;	// is set to the number of magic ammo player has
	public string[] Tags;

	[FMODUnity.EventRef]
	private string footstep1 = "event:/HC_Footsteps_Stone1";
	private string footstep2 = "event:/HC_Footsteps_Stone2";
	private string footstep3 = "event:/HC_Footsteps_Stone3";

	private List<string> FMOD_footstepSounds;
	private int FMOD_randNum;

	Ray shootRay;
	void Awake() {
		hasKey = false;
		FMOD_footstepSounds = new List<string>();
		FMOD_footstepSounds.Add(footstep1);
		FMOD_footstepSounds.Add(footstep2);
		FMOD_footstepSounds.Add(footstep3);

		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		// Set up references.
		playerRigidbody = GetComponent<Rigidbody>();

		if (transform.name == "Mage(Clone)") {
			isDefault = true;
			isMagic = false;
			isWeapon = false;
		} else if (transform.name == "Gunner(Clone)") {
			isDefault = true;
			isMagic = false;
			isWeapon = false;
		}
	}


	void Update() {

		if (isDefault) {
			if (transform.name == "Mage(Clone)") {
				anim.SetBool ("hasGun", false);
				anim.SetBool ("hasMagic", true);
			} else if (transform.name == "Gunner(Clone)") {
				anim.SetBool ("hasGun", true);
				anim.SetBool ("hasMagic", false);
			}
		} else if (isMagic) {
			anim.SetBool ("hasGun", false);
			anim.SetBool ("hasMagic", true);
		} else if (isWeapon) {
			anim.SetBool ("hasGun", true);
			anim.SetBool ("hasMagic", false);
		}

	}


	void FixedUpdate() {
		// Store the input axes.
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		//Rotate with mouse point
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Move the player around the scene.
		anim.SetFloat("Valx", h);
		anim.SetFloat("Valy", v);
		// Debug.Log(h);
		//Debug.Log(v);
		Move(h, v);
		if (h != 0 || v != 0) {
			anim.SetBool("isRunning", true);
		} else {
			//anim.SetBool ("isWalking", false);
			anim.SetBool("isRunning", false);
		}
		// Turn the player to face the mouse cursor.
		Turning();

		// Animate the player.

	}

	void Move(float h, float v) {
		// Set the movement vector based on the axis input.
		movement.Set(h, 0f, v);

		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;

		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition(transform.position + movement);
	}
	//    void OnCollisionEnter(Collision col) {
	//        if (col.gameObject.tag == "Slim") {
	//            playerHealth -= 10;
	//        } else if (col.gameObject.tag == "Goblin") {
	//			
	//            playerHealth -= 15;
	//        } else if (col.gameObject.tag == "Boss") {
	//            playerHealth -= 25;
	//        }
	//		Debug.Log (playerHealth);
	//    }
	void ApplyDamage( int damage)
	{ 
		playerHealth -= damage;
		Debug.Log (playerHealth);
	}
	void Turning() {
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			anim.SetFloat("RotY", newRotation.y);
			anim.SetFloat("RotW", newRotation.w);

			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation(newRotation);

		}
	}

	void OnTriggerStay(Collider other) {
		if (Input.GetKeyDown(KeyCode.E)) {
			if (other.gameObject.CompareTag("Key")) {
				hasKey = true;
				Destroy(other.gameObject);
			} else if (other.gameObject.CompareTag("Med")) {
				playerHealth += 10;
				Destroy(other.gameObject);
			} else if (other.gameObject.CompareTag("Weapon")) {
				isDefault = false;
				isMagic = false;
				isWeapon = true;
				ammo += 10;
				PrepareWeapon();
				anim.SetBool("hasGun", true);
				anim.SetBool("hasMagic", false);
				Destroy(other.gameObject);
			} else if (other.gameObject.CompareTag("Magic")) {
				isDefault = false;
				isMagic = true;
				isWeapon = false;
				magicAmmo += 10;
				PrepareMagic();
				anim.SetBool("hasGun", false);
				anim.SetBool("hasMagic", true);
				Destroy(other.gameObject);
			}
		}
	}

	void PrepareWeapon() {
		foreach (Transform child in transform) {
			if (child.CompareTag("WeaponGroup")) {
				foreach (Transform gchild in child) {
					if (gchild.name == "MagicBarrelEnd") {
						gchild.gameObject.SetActive (false);
					} else if (gchild.name == "GunBarrelEnd") {
						gchild.gameObject.SetActive (true);
					} else if (gchild.name == "Default") {
						gchild.gameObject.SetActive (false);
					}
				}
			}
		}
	}


	void PrepareMagic() {
		foreach (Transform child in transform) {
			if (child.CompareTag("WeaponGroup")) {
				foreach (Transform gchild in child) {
					if (gchild.name == "MagicBarrelEnd") {
						gchild.gameObject.SetActive(true);
					} else if (gchild.name == "GunBarrelEnd") {
						gchild.gameObject.SetActive(false);
					} else if (gchild.name == "Default") {
						gchild.gameObject.SetActive (false);
					}
				}
			}
		}
	}

	void FMOD_makeFootstep() {
		if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
			FMOD_randNum = Random.Range(0, 3);
			FMODUnity.RuntimeManager.PlayOneShot(FMOD_footstepSounds[FMOD_randNum]);
		}
	}
}