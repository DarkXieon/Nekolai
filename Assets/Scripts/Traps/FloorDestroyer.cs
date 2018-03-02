using UnityEngine;
using UnityEditor;
using System.Linq;

public class FloorDestroyer : MonoBehaviour, ITrap
{
    [SerializeField]
    private GameObject _toDestroy;

    public void Activate()
    {
        var rigidbody2D = this.GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = false;

        Debug.Log("Activated");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");

        Debug.Log(other.gameObject.name);

        if(other.gameObject.Equals(_toDestroy))
        {
            Destroy(this._toDestroy);
            Destroy(this.gameObject);
        }
        /*
        other.gameObject.SetActive(false);

        Destroy(other.gameObject);
        Destroy(this.gameObject);
        */
    }
}