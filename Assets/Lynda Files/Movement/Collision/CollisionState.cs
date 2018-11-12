using UnityEngine;
using System.Collections;

public class CollisionState : MonoBehaviour
{
	public LayerMask CollisionLayer;
    public bool Standing;
    public bool HittingRightWall;
    public bool HittingLeftWall;
    public Vector2 BottomPositionOffset = Vector2.zero;
    public Vector2 LeftBoxSize = Vector2.zero;
    public Vector2 LeftBoxPositionOffset = Vector2.zero;
    public Vector2 RightBoxSize = Vector2.zero;
    public Vector2 RightBoxPositionOffset = Vector2.zero;
    public float BottomCollisionRadius = 10f;
    public Color DebugCollisionColor = Color.red;
    
	private void FixedUpdate()
    {
		var position = this.BottomPositionOffset;
		position.x += this.transform.position.x;
		position.y += this.transform.position.y;

        this.Standing = Physics2D.OverlapCircle(position, this.BottomCollisionRadius, this.CollisionLayer) != null;

        position = this.RightBoxPositionOffset;
        position.x += this.transform.position.x;
        position.y += this.transform.position.y;

        this.HittingRightWall = Physics2D.OverlapBox(position, this.RightBoxSize, 0, this.CollisionLayer) != null;

        position = this.LeftBoxPositionOffset;
        position.x += this.transform.position.x;
        position.y += this.transform.position.y;

        this.HittingLeftWall = Physics2D.OverlapBox(position, this.LeftBoxSize, 0, this.CollisionLayer) != null;
    }

    private void OnDrawGizmos()
    {
		Gizmos.color = DebugCollisionColor;

		var position = this.BottomPositionOffset;
		position.x += transform.position.x;
		position.y += transform.position.y;

		Gizmos.DrawWireSphere(position, this.BottomCollisionRadius);

        position = this.LeftBoxPositionOffset;
        position.x += this.transform.position.x;
        position.y += this.transform.position.y;

        Gizmos.DrawWireCube(position, this.LeftBoxSize);

        position = this.RightBoxPositionOffset;
        position.x += this.transform.position.x;
        position.y += this.transform.position.y;

        Gizmos.DrawWireCube(position, this.RightBoxSize);
    }
}
