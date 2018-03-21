using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class ColliderSorter : MonoBehaviour
{
    public Collider2D HeadCollider { get; private set; }

    public Collider2D FootCollider { get; private set; }

    public Collider2D[] OtherColliders { get; private set; }

    // Use this for initialization
    void Start()
    {
        IOrderedEnumerable<Collider2D> attachedColliders = this.GetComponents<Collider2D>()
            .ToList()
            .OrderByDescending(collider => collider.bounds.center.y);

        HeadCollider = attachedColliders.First();

        FootCollider = attachedColliders.Last();

        OtherColliders = attachedColliders.Where(collider => collider != HeadCollider && collider != FootCollider).ToArray();
    }
}
