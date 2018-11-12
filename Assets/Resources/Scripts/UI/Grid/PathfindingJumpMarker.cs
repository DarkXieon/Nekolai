using UnityEngine;

public class PathfindingJumpMarker : PathfindingMarker
{
    public HorizontalDirection JumpDirection;

    protected override Color gizmoColor { get { return Color.yellow; } }
}
