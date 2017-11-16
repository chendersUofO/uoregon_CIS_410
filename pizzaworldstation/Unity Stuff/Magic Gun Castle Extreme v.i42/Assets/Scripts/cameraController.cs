using UnityEngine;
using System.Collections;
/*
* Greatly influenced by official Unity tutorial: Stealth game - cameraMovement.cs
*/

public class cameraController : MonoBehaviour {

    private GameObject player;
    public GameObject gunner;
    public GameObject mage;
    static public int playerChoice;
    public static GameObject playerToken;
    private Vector3 offset;
    public float smooth = 1.5f;
    private Transform playerTransform;
    private Vector3 relCameraPos;
    private float relCameraPosMag; 
    private Vector3 newPos;

    // Use this for initialization
    void Start () {
        offset = new Vector3 (0, 6, -3);

        if (playerChoice == 1) {
            player = gunner;
        } else if (playerChoice == 2) {
            player = mage;
        }
        playerToken = Instantiate(player);

        playerTransform = playerToken.transform;

        relCameraPos = playerTransform.position + offset;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
    }
    
    // Update is called once per frame
    void LateUpdate () {
        // The standard position of the camera is the relative position of the camera from the player.
        Vector3 standardPos = playerTransform.position + relCameraPos;
        
        // The abovePos is directly above the player at the same distance as the standard position.
        Vector3 abovePos = playerTransform.position + Vector3.up * relCameraPosMag;
        
        // An array of 5 points to check if the camera can see the player.
        Vector3[] checkPoints = new Vector3[5];
        
        // The first is the standard position of the camera.
        checkPoints[0] = standardPos;
        
        // The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        
        // The last is the abovePos.
        checkPoints[4] = abovePos;
        
        // Run through the check points...
        for(int i = 0; i < checkPoints.Length; i++)
        {
            // ... if the camera can see the player...
            if(ViewingPosCheck(checkPoints[i]))
                // ... break from the loop.
                break;
        }
        
        // Lerp camera's position between its current position and its new position.
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        
        // Make sure the camera is looking at the player.
        SmoothLookAt();
    }
    
    
    bool ViewingPosCheck (Vector3 checkPos)
    {
        RaycastHit hit;
        
        // If a raycast from the check position to the player hits something...
        if(Physics.Raycast(checkPos, playerTransform.position - checkPos, out hit, relCameraPosMag))
            // ... if it is not the player...
            if(hit.transform != playerTransform)
                // This position isn't appropriate.
                return false;
        
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        newPos = checkPos;
        return true;
    }
    
    
    void SmoothLookAt ()
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = playerTransform.position - transform.position;
        
        // Create a rotation based on the relative position of the player being the forward vector.
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
        
        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
 
    }
}
