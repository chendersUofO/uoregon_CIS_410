using UnityEngine;
using System.Collections;

public class FMOD_FootstepTrigger : MonoBehaviour {
    [FMODUnity.EventRef]
    private string footstep = "event:/HC_Footsteps_Stone";

    // Use this for initialization
    void Start () {
        FMODUnity.RuntimeManager.PlayOneShot(footstep);
    }
}
