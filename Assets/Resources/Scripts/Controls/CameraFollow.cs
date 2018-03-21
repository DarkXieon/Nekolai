using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ThingToFollow; // the object to track
    public float XOffset;
    public float YOffset;
    //[SerializeField] private float margin; // the margin of error to allow for camera panning

    private void LateUpdate()
    {
        // don't run the script once the thing to follow has disappeared
        if (ThingToFollow == null)
        {
            return;
        }

       // if (ThingToFollow.position.x < Camera.main.transform.position.x - margin || ThingToFollow.position.x > Camera.main.transform.position.x + margin)
        {
            //Vector3.Lerp(Camera.main.transform.position, new Vector3(ThingToFollow.position.x, ThingToFollow.position.y, Camera.main.transform.position.z), 0.5f);
            var cameraPosition = new Vector3(ThingToFollow.position.x + XOffset, ThingToFollow.position.y + YOffset, Camera.main.transform.position.z);

            Camera.main.transform.position = cameraPosition;
        }
    }


}
