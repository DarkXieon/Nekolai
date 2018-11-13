using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class PathfindingMarker : MonoBehaviour
{
    public Collider2D ConnectingPlatform;

    protected abstract Color gizmoColor { get; }
    
    private void Start()
    {
        if (!GetComponent<Collider2D>().isTrigger)
            Debug.LogError("Set collider on Pathfinding Marker to be a trigger");
    }

    //private void OnDrawGizmos()
    //{
    //    Collider2D markerCollider = GetComponent<Collider2D>();

    //    Gizmos.color = gizmoColor;
    //    Gizmos.DrawWireCube(markerCollider.bounds.center, markerCollider.bounds.size);
    //}
}
