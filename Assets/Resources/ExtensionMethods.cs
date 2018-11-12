using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class ExtensionMethods
{
    public static List<Transform> GetAllChildTransforms(this Transform transform)
    {
        var children = new List<Transform>();

        foreach(Transform childTransform in transform)
        {
            children.Add(childTransform);

            if(childTransform.childCount > 0)
            {
                children.AddRange(childTransform.GetAllChildTransforms());
            }
        }

        return children;
    }

    public static int GetNextPositiontWrap<TElement>(this IEnumerable<TElement> enumerable, int currentPosition)
    {
        var hasAdditionalElements = currentPosition + 1 < enumerable.Count();

        var nextPosition = hasAdditionalElements
            ? currentPosition + 1
            : 0;

        return nextPosition;
    }

    public static int GetPreviousPositiontWrap<TElement>(this IEnumerable<TElement> enumerable, int currentPosition)
    {
        var hasAdditionalElements = currentPosition - 1 >= 0;

        var nextPosition = hasAdditionalElements
            ? currentPosition - 1
            : enumerable.Count() - 1;

        return nextPosition;
    }

    public static Vector2? GetClosestPlatformEdge(this Vector2 currentPosition)
    {
        var rayHit = Physics2D.Raycast(currentPosition, Vector2.down, 9f);

        //var rightMostEdge = rayHit.collider.GetRightMostConnectedCollider().GetRightEdge();
        if (rayHit.collider == null)
        {
            Debug.LogError("Null");
        }
        var rightMostEdge = GetRightMostConnectedCollider(rayHit.collider).GetRightEdge();

        var leftMostEdge = rayHit.collider.GetLeftMostConnectedCollider().GetLeftEdge();

        var closestEdge = rightMostEdge.x > leftMostEdge.x
            ? rightMostEdge
            : leftMostEdge;

        return closestEdge;
    }

    public static Queue<Vector2> GetSafePathDown(this Vector2 currentPosition, Vector2 leadingTo)
    {
        var collider = currentPosition.GetPlatformBelow();
        //var rayHit = Physics2D.Raycast(currentPosition, Vector2.down, 12f);

        if(collider != null)//rayHit.collider != null)
        {
            //Debug.Log(collider.gameObject.name);// == "Test 2");
            /*
            Collider2D[] colliders = new Collider2D[1];

            var amount = rayHit.collider.OverlapCollider(new ContactFilter2D(), colliders);

            Debug.LogError(amount);

            var rightEdge = rayHit.collider.GetColliderToRight();
            */

            var pathVectors = new Queue<Vector2>();

            var rightMostEdge = collider.GetRightMostConnectedCollider().GetRightEdge();
            //Debug.LogError(rightMostEdge);
            //var rightMostEdge = rayHit.collider.GetRightMostConnectedCollider().GetRightEdge();

            var leftMostEdge = collider.GetLeftMostConnectedCollider().GetLeftEdge();
            //Debug.LogError(collider.GetLeftMostConnectedCollider().gameObject.name == "Test");
            bool rightNull = rightMostEdge == null;
            
            bool leftNull = leftMostEdge == null;

            var newRightEdge = new Vector2(rightMostEdge.x + 4f, rightMostEdge.y);

            var newLeftEdge = new Vector2(leftMostEdge.x - 4f, leftMostEdge.y);

            var underNewRightEdge = newRightEdge.GetPlatformBelow();

            var underNewLeftEdge = newLeftEdge.GetPlatformBelow();

            var underRightEdge = rightMostEdge.GetPlatformBelow();
            //Debug.LogError(underRightEdge.ToPathingVector());
            var underLeftEdge = leftMostEdge.GetPlatformBelow();
            //Debug.LogError(underLeftEdge.ToPathingVector());

            var reachableFromRight = underRightEdge == null
                ? false
                : underRightEdge.IsReachable(leadingTo.x);
            //Debug.LogError(reachableFromRight);
            var reachableFromLeft = underLeftEdge == null
                ? false
                : underLeftEdge.IsReachable(leadingTo.x);
            //Debug.LogError(underLeftEdge.ToPathingVector());
            var isRightCloser = Mathf.Abs(underRightEdge.bounds.center.x - leadingTo.x) < Mathf.Abs(currentPosition.x - leadingTo.x);

            var isLeftCloser = Mathf.Abs(underLeftEdge.bounds.center.x - leadingTo.x) < Mathf.Abs(currentPosition.x - leadingTo.x);

            var underRightEdgeVector = underRightEdge.ToPathingVector(true);

            var underNewRightEdgeVector = underNewRightEdge.ToPathingVector(true);

            var underLeftEdgeVector = underLeftEdge.ToPathingVector(false);

            var underNewLeftEdgeVector = underNewLeftEdge.ToPathingVector(false);

            //Debug.LogError(underRightEdge);

            //var underRightIsCloser = underRightEdge
            //rightMostEdge = new Vector2(rightMostEdge.x + 10, rightMostEdge.y);

            //leftMostEdge = new Vector2(leftMostEdge.x - 10, leftMostEdge.y);

            if (reachableFromLeft && reachableFromRight)
            {
                var closestEdge = isRightCloser//Math.Abs(currentPosition.x - rightMostEdge.x) > Math.Abs(currentPosition.x - leftMostEdge.x)
                ? rightMostEdge
                : leftMostEdge;

                pathVectors.Enqueue(closestEdge.ToPathingVector());

                if (isRightCloser)//closestEdge == rightMostEdge)
                {
                    Debug.Log("RIGHT is closer");
                    pathVectors.Enqueue(newRightEdge.ToPathingVector());

                    pathVectors.Enqueue(underNewRightEdgeVector);

                    if(underNewRightEdgeVector.y != underRightEdgeVector.y)
                    {
                        pathVectors.Enqueue(underRightEdgeVector);
                    }
                }
                else
                {
                    Debug.Log("Left is closer");
                    pathVectors.Enqueue(newLeftEdge.ToPathingVector());

                    pathVectors.Enqueue(underNewLeftEdgeVector);
                    //Debug.Log("UnderNewLeftEdge " + underNewLeftEdgeVector);
                    if (underNewLeftEdgeVector.y != underLeftEdgeVector.y)
                    {
                        pathVectors.Enqueue(underLeftEdgeVector);
                    }
                }
                pathVectors.PrintQueue();
                //Debug.LogError("reachableFromLeft && reachableFromRight " + pathVectors.ToString());
                return pathVectors;
            }
            else if (!reachableFromLeft && reachableFromRight)
            {
                pathVectors.Enqueue(rightMostEdge.ToPathingVector());

                pathVectors.Enqueue(newRightEdge.ToPathingVector());

                pathVectors.Enqueue(underNewRightEdgeVector);

                if (underNewRightEdgeVector.y != underRightEdgeVector.y)
                {
                    pathVectors.Enqueue(underRightEdgeVector);
                }
                //Debug.LogError(pathVectors.ToString());
                return pathVectors;
            }
            else if (reachableFromLeft && !reachableFromRight)
            {
                pathVectors.Enqueue(leftMostEdge.ToPathingVector());

                pathVectors.Enqueue(newLeftEdge.ToPathingVector());

                pathVectors.Enqueue(underNewLeftEdgeVector);

                if (underNewLeftEdgeVector.y != underLeftEdgeVector.y)
                {
                    pathVectors.Enqueue(underLeftEdgeVector);
                }
                //Debug.LogError(pathVectors.ToString());
                return pathVectors;
            }
            else if (isRightCloser)
            {
                pathVectors.Enqueue(rightMostEdge.ToPathingVector());

                pathVectors.Enqueue(newRightEdge.ToPathingVector());

                pathVectors.Enqueue(underNewRightEdgeVector);

                if (underNewRightEdgeVector.y != underRightEdgeVector.y)
                {
                    pathVectors.Enqueue(underRightEdgeVector);
                }
                //Debug.LogError(pathVectors.ToString());
                return pathVectors;
            }
            else if (isLeftCloser)
            {
                pathVectors.Enqueue(leftMostEdge.ToPathingVector());

                pathVectors.Enqueue(newLeftEdge.ToPathingVector());

                pathVectors.Enqueue(underNewLeftEdgeVector);

                if (underNewLeftEdgeVector.y != underLeftEdgeVector.y)
                {
                    pathVectors.Enqueue(underLeftEdgeVector);
                }
                //Debug.LogError(pathVectors.ToString());
                return pathVectors;
            }   
            else
            {
                Debug.LogError("This should never happen");

                return null;
            }
        }
        else
        {
            Debug.LogError("This should never happen");

            return null;
        }
    }

    public static Collider2D GetColliderToRight(this Collider2D collider)
    {
        /*
        if(collider != null)
        {
            Collider2D[] colliders = new Collider2D[10];

            var amount = collider.OverlapCollider(new ContactFilter2D(), colliders);

            if (amount > 0)
            {
                var found = colliders
                .ToList()
                .Where(foundCollider => foundCollider != null)
                .Where(foundCollider => foundCollider.transform.position.x > collider.transform.position.x)
                .OrderBy(foundCollider => foundCollider.transform.position.x)
                .FirstOrDefault();

                //Debug.LogError(found == null);

                return found;
            }
        }

        return null;
        */

        if (collider != null)
        {
            //Debug.LogError(collider.gameObject.name);

            var edge = collider.GetRightEdge();

            //RaycastHit2D[] hitColliders = new RaycastHit2D[1];

            //collider.Raycast(Vector2.right, hitColliders);

            var mask = LayerMask.GetMask("Terrain");
            
            var hitCollider = Physics2D.Raycast(edge, Vector2.right, 10f, mask);
            
            if (hitCollider.collider != null)
            {
                return hitCollider.collider;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.LogError("This shouldn't happen.");

            return null;
        }

        //Debug.LogError(hitCollider.collider == null);

        
    }

    public static Collider2D GetColliderToLeft(this Collider2D collider)
    {
        if (collider != null)
        {
            //collider.get

            var edge = collider.GetLeftEdge();

            var mask = LayerMask.GetMask("Terrain");

            var hitCollider = Physics2D.Raycast(edge, Vector2.left, 10f, mask);

            if (hitCollider.collider != null)
            {
                return hitCollider.collider;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.LogError("This shouldn't happen.");

            return null;
        }

        /*
        if (collider != null)
        {
            Collider2D[] colliders = new Collider2D[10];

            var amount = collider.OverlapCollider(new ContactFilter2D(), colliders);

            if (amount > 0)
            {
                if(amount > 10)
                {
                    Debug.LogError("This shouldn't happen.");
                }

                var found = colliders
                .ToList()
                .Where(foundCollider => foundCollider != null)
                .Where(foundCollider => foundCollider.transform.position.x > collider.transform.position.x)
                .OrderBy(foundCollider => foundCollider.transform.position.x)
                .FirstOrDefault();

                //Debug.LogError(found == null);

                return found;
            }
            
    

        return null;
        /*
        var edge = collider.GetLeftEdge();
        
        var hitCollider = Physics2D.Raycast(edge, Vector2.left, 1f, 0, 0, 0); //add mask specification


        //Debug.LogError(hitCollider.collider == null);


        return hitCollider.collider;
        */
    }
    
    public static Collider2D GetRightMostConnectedCollider(this Collider2D collider, int count = 0)
    {
        if (collider != null)
        {
            Collider2D nextCollider = collider.GetColliderToRight();

            if (nextCollider != null)
            {
                return GetRightMostConnectedCollider(nextCollider);
            }
            else
            {
                return collider;
            }
        }
        else
        {
            Debug.LogError("It's null ");
        }

        return null;
    }
    /*
    public static Collider2D GetRightMostConnectedCollider(Collider2D collider, int count = 0)
    {

        if (collider != null)
        {
            Collider2D nextCollider = collider.GetColliderToRight();

            if (nextCollider != null)
            {
                return GetRightMostConnectedCollider(nextCollider);
            }
            else
            {
                return collider;
            }
        }
        else
        {
            Debug.LogError("It's null ");
        }

        return null;
    }
    */
    public static Collider2D GetLeftMostConnectedCollider(this Collider2D collider)
    {
        if(collider != null)
        {
            Collider2D nextCollider = collider.GetColliderToLeft();

            if (nextCollider != null)
            {
                return nextCollider.GetLeftMostConnectedCollider();
            }
            else
            {
                return collider;
            }
        }

        return null;
    }

    public static Vector2 GetLeftEdge(this Collider2D collider)
    {
        var leftEdge = collider.bounds.center.x - collider.bounds.size.x / 2.0f;

        var leftEdgeFull = new Vector2(leftEdge, collider.bounds.center.y);

        return leftEdgeFull;
    }

    public static Vector2 GetRightEdge(this Collider2D collider)
    {
        var rightEdge = collider.bounds.center.x + collider.bounds.size.x / 2.0f;//+ 3.5f;

        var rightEdgeFull = new Vector2(rightEdge, collider.bounds.center.y);

        //Debug.LogError(collider.bounds.size);

        return rightEdgeFull;
    }

    public static Collider2D GetPlatformBelow(this Vector2 position, float rayDistance = 100f)
    {
        var mask = LayerMask.GetMask("Terrain");

        var rayHit = Physics2D.Raycast(position, Vector2.down, rayDistance, mask);
        
        return rayHit.collider;
    }

    public static bool IsReachable(this Collider2D collider, float xPosition)
    {
        if(collider == null)
        {
            Debug.LogError("Null");
        }
        //var rightMostEdge = collider.GetRightMostConnectedCollider().GetRightEdge().x;

        var rightMostEdge = GetRightMostConnectedCollider(collider).GetRightEdge().x;

        var leftMostEdge = collider.GetLeftMostConnectedCollider().GetLeftEdge().x;

        return xPosition > leftMostEdge && xPosition < rightMostEdge;
    }

    public static float? GetDistanceDown(this Vector2 position, float rayDistance = 100f)
    {
        var rayHit = Physics2D.Raycast(position, Vector2.down, rayDistance);

        if(rayHit.collider != null)
        {
            var distance = position.y - rayHit.distance;

            return distance;
        }
        else
        {
            return null;
        }
    }

    public static Vector2 ToPathingVector(this Collider2D collider, bool adjustRight = true)
    {/*
        var xValue = adjustRight
            ? collider.GetRightEdge().x
            : collider.GetLeftEdge().x;
            */
        return new Vector2(/*xValue*/collider.bounds.center.x, collider.bounds.center.y + 5.5f);
    }

    public static Vector2 ToPathingVector(this Vector2 vector)
    {
        return new Vector2(vector.x, vector.y + 5.5f);
    }

    public static void PrintQueue(this Queue<Vector2> queue)
    {
        queue.ToList().ForEach(vector => Debug.Log(vector));
    }

    public static void PrintEnumerable<T>(this IEnumerable<T> enumerable)
    {
        enumerable.ToList().ForEach(vector => Debug.Log(vector));
    }

    public static Vector3 ToVector3(this Vector2 vector2, int z = 0)
    {
        return new Vector3(vector2.x, vector2.y, z);
    }
}