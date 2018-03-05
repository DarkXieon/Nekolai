using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform thingToFollow; // the object to track
    //[SerializeField] private float margin; // the margin of error to allow for camera panning

    private void LateUpdate()
    {
        // don't run the script once the thing to follow has disappeared
        if (thingToFollow == null)
        {
            return;
        }

       // if (thingToFollow.position.x < Camera.main.transform.position.x - margin || thingToFollow.position.x > Camera.main.transform.position.x + margin)
        {
            //Vector3.Lerp(Camera.main.transform.position, new Vector3(thingToFollow.position.x, thingToFollow.position.y, Camera.main.transform.position.z), 0.5f);

           Camera.main.transform.position = new Vector3(thingToFollow.position.x, thingToFollow.position.y, Camera.main.transform.position.z);
        }
    }


}
