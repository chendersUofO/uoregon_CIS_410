using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	public int defaultDamage = 10;

	public float timeBetweenBullets = 0.15f;        // Time between each shot
	public float range = 100f;                      // The distance in which the weapon can be shot at

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	Light gunLight;                                 // Reference to the light component.
	public Light faceLight;
	float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
	public float attackCoolDown;					// slows down projectiles
	public float attackRate = 10f;

	[FMODUnity.EventRef]
	private string rifle = "event:/HC_Weapon_Rifle";

	void Awake() {
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask("Shootable");

		// Set up the references.
		gunParticles = GetComponent<ParticleSystem>();
		gunLine = GetComponent<LineRenderer>();

		gunLight = GetComponent<Light>();

		attackCoolDown = 0;

	}


	void Update() {
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// If the Fire1 button (left-click) is being press and it's time to fire...
		if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
			if (PlayerController.isMagic) {
				ShootMagic ();
			} else if (PlayerController.isWeapon) {
				ShootWeapon ();
				FMODUnity.RuntimeManager.PlayOneShot (rifle);
			} else {
				Shoot ();
			}
		} else if (Input.GetKeyDown(KeyCode.LeftShift)) {
			SwitchCurrentProjectile ();
		}

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			// ... disable the effects.
			DisableEffects();
		}

		if (attackCoolDown > 0) {
			attackCoolDown -= Time.deltaTime;
		}
		else 
		{
			attackCoolDown = attackRate;
		}

	}

	public void DisableEffects() {
		// Disable the line renderer and the light.
		gunLine.enabled = false;
		faceLight.enabled = false;
		gunLight.enabled = false;
	}

	// order goes [default, magic, weapon]


	void SwitchCurrentProjectile() {
		// magic -> weapon, if ammo >0
		if (PlayerController.isMagic) {
			if (PlayerController.ammo > 0) {
				SwitchToWeapon ();
			} else {
				SwitchToDefault ();
			}
		} 
		// weapon -> default
		else if (PlayerController.isWeapon) {
			SwitchToDefault ();
		}
		// default -> magic or default- > weapon, if ammo > 0
		else if (PlayerController.isDefault) {
			if (PlayerController.magicAmmo > 0) {
				SwitchToMagic ();
			} else if (PlayerController.ammo > 0) {
				SwitchToWeapon ();
			} 
		} 
	}


	void Shoot() {
		
		timer = 0f;

		// Play the gun shot audioclip.
		//gunAudio.Play ();

		// Enable the lights.
		gunLight.enabled = true;
		faceLight.enabled = true;

		// Stop the particles from playing if they were, then start the particles.
		gunParticles.Stop();
		gunParticles.Play();

		// Enable the line renderer and set it's first position to be the end of the gun.
		if (PlayerController.isWeapon) {
			gunLine.enabled = true;
			gunLine.SetPosition(0, transform.position);
		}

		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.parent.parent.position;
		shootRay.direction = transform.parent.parent.forward;

		if (Physics.Raycast(shootRay, out shootHit, 100, shootableMask)) {
			print ("found enemy!");
			EnemyHealth enemyHealth = shootHit.transform.GetComponent<EnemyHealth>();
			// If the EnemyHealth component exist...
			if (enemyHealth != null) {
				// ... the enemy should take damage.
				enemyHealth.TakeDamage(defaultDamage, shootHit.point);
			}
			if (PlayerController.isWeapon) {
				gunLine.SetPosition(1, shootHit.point);
			}
		}
		// If the raycast didn't hit anything on the shootable layer...
		else {
			if (PlayerController.isWeapon) {
				gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
			}
		}
	}

	void ShootWeapon() {

		Shoot ();
		if (PlayerController.isWeapon && PlayerController.ammo > 0) {
			PlayerController.ammo--;
		} else if (PlayerController.ammo == 0) {
			SwitchToDefault ();
		}
	}

	void ShootMagic() {

		Shoot ();
		if (PlayerController.isMagic && PlayerController.magicAmmo > 0) {
			PlayerController.magicAmmo--;
		} else if (PlayerController.magicAmmo == 0)  {
			SwitchToDefault ();
		}
	}

	void SwitchToDefault() {

		PlayerController.isDefault = true;
		PlayerController.isMagic = false;
		PlayerController.isWeapon = false;

		Transform parent = transform.parent;

		foreach (Transform child in parent) {
			// disable the weapon
			if (child.name == "GunBarrelEnd") {
				child.gameObject.SetActive(false);
			}
			// disable the magic
			else if (child.name == "MagicBarrelEnd") {
				child.gameObject.SetActive(false);
			} 
			// re-enable the default projectile
			else if (child.name == "Default") {
				child.gameObject.SetActive (true);
			}
		}
	}

	void SwitchToMagic() {

		PlayerController.isDefault = false;
		PlayerController.isMagic = true;
		PlayerController.isWeapon = false;

		Transform parent = transform.parent;

		foreach (Transform child in parent) {
			// disable the weapon
			if (child.name == "GunBarrelEnd") {
				child.gameObject.SetActive(false);
			}
			// re-enable the magic
			else if (child.name == "MagicBarrelEnd") {
				child.gameObject.SetActive(true);
			} 
			// disable default
			else if (child.name == "Default") {
				child.gameObject.SetActive (false);
			}
		}
	}

	void SwitchToWeapon() {

		PlayerController.isDefault = false;
		PlayerController.isWeapon = true;
		PlayerController.isMagic = false;

		Transform parent = transform.parent;

		foreach (Transform child in parent) {
			// disable the weapon
			if (child.name == "GunBarrelEnd") {
				child.gameObject.SetActive (true);
			}
			// re-enable the magic
			else if (child.name == "MagicBarrelEnd") {
				child.gameObject.SetActive (false);
			} 
			// disable default
			else if (child.name == "Default") {
				child.gameObject.SetActive (false);
			}
		}
	}
}