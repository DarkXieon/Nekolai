using UnityEngine;
using System.Collections;

public class ReportChildCollisionsToParent : MonoBehaviour
{
    public GameObject Parent { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Parent Set");

        Parent.SendMessage("OnCollisionEnter2D", collision, SendMessageOptions.DontRequireReceiver);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Parent.SendMessage("OnCollisionStay2D", collision, SendMessageOptions.DontRequireReceiver);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Parent.SendMessage("OnCollisionExit2D", collision, SendMessageOptions.DontRequireReceiver);
    }
}
