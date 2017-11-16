using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;

public class FMOD_FireFlicker : MonoBehaviour {
    [EventRef]
    private string fire1 = "event:/HC_Fire1";
    private string fire2 = "event:/HC_Fire2";
    private string fire3 = "event:/HC_Fire3";
    private string fire4 = "event:/HC_Fire4";
    private string fire5 = "event:/HC_Fire5";
    //private string fire5 = "event:/HC_Weapon_rifle";

    private List<string> fireSounds;
    private int randNum;
    private GameObject pointLight;
 


    void Start() {
        //light = GetComponent<GameObject>("Point light");
        pointLight = gameObject;

        fireSounds = new List<string>();

        fireSounds.Add(fire1);
        fireSounds.Add(fire2);
        fireSounds.Add(fire3);
        fireSounds.Add(fire4);
        fireSounds.Add(fire5);



        randNum = Random.Range(0, 5);

        //RuntimeManager.PlayOneShot(fireSounds[randNum]);

        RuntimeManager.PlayOneShotAttached(fireSounds[randNum], pointLight);
    }
}
