using UnityEngine;
//using SceneManagement;

public class PortalController : MonoBehaviour {
	public bool requireKey = true;
	public GameObject player;


	void OnTriggerEnter (Collider other) {
        // If the triggering gameobject is the player...
		if(other.gameObject.tag == "Player") {
            // ... if this door requires a key...
            if(requireKey) {
                // ... if the player has the key...
				if(PlayerController.hasKey) {
					Application.LoadLevel("Main");
                }
            } else {
                // If the door doesn't require a key, increase the count of triggering objects.
				Application.LoadLevel("Boss");
            }
        }
    }
}