using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Path
{
    private Queue<Vector2> _path;

    private Vector2 _current;

    private bool _canUpdate;
    
    public Path(Vector2 pathFrom, Vector2 pathTo)
    {
        _path = new Queue<Vector2>();
        
        this.UpdatePath(pathFrom, pathTo);
    }

    public bool UpdatePath(Vector2 pathFrom, Vector2 pathTo)
    {
        var finalPath = FindFirstPathsTo(pathFrom, pathTo);

        if(_path.Any() && finalPath.Peek().x == this.CurrentTarget().x)
        {
            return false;
        }
        else
        {
            _path = finalPath;

            _current = finalPath.Peek();

            return true;
        }
        /*
        var current = _current.HasValue
            ? _current.Value
            : pathTo;

        if (finalPath.x == current.x && finalPath.y == current.y)
        {
            return false;
        }
        else
        {
            _path.Clear();
            //_path.Enqueue(finalPath);

            Vector2? previousPath = null;

            while (!previousPath.HasValue || finalPath != previousPath.Value)
            {
                previousPath = finalPath;
                //Debug.LogError(previousPath);
                _path.Enqueue(previousPath.Value);

                finalPath = FindFirstPathsTo(finalPath, pathTo);
            }

            _path.Enqueue(finalPath);

            if(!_path.Any())
            {
                Debug.LogError("fsfd");
            }

            _current = _path.Peek();
            
            return true;
        }
        */
    }

    public Vector2 CurrentTarget()
    {
        /*
        if(!_current.HasValue)
        {
            Debug.LogError(StackTraceUtility.ExtractStackTrace());

            return _path.Peek();
        }
        */
        if(_path.Any())
        {
            return _path.Peek();
        }
        else
        {
            return _current;
        }
    }

    public Vector2 AdvancePath()
    {
        if(_path.Any())
        {
            _path.Dequeue();

            if (_path.Any())
            {
                _current = _path.Peek();
            }
        }
        
        return this.CurrentTarget();
    }

    private Queue<Vector2> FindFirstPathsTo(Vector2 pathFrom, Vector2 pathTo)
    {
        var pathingUp = pathTo.y - pathFrom.y >= 1f;
        var pathingDown = pathFrom.y - pathTo.y >= 1f;
        var pathingRight = pathTo.x - pathFrom.x >= 1f;
        var pathingLeft = pathFrom.x - pathTo.x >= 1f;

        //float finalX = 0;
        //float finalY = 0;

        Queue<Vector2> finalPath = new Queue<Vector2>();

        if (pathingUp)
        {
            //need to write
            Debug.LogError("Code missing");
        }
        else if (pathingDown)
        {
            var possiblePathingEdges = pathFrom.GetSafePathDown(pathTo);
            //Debug.LogError(possiblePathingEdge);
            if (possiblePathingEdges != null)
            {
                finalPath = possiblePathingEdges;

                
                /*
                var movement = this.GetMovementFromEdge(possiblePathingEdge.Value);

                finalX = movement.x;

                //finalY = movement.y - 8f;
                
                var endPosition = movement.GetDistanceDown();

                if (endPosition.HasValue)
                {
                    //Debug.LogError(movement.y);
                    finalY = endPosition.Value + 6f;
                }
                else
                {
                    Debug.LogError("This should never happen");
                }
                */
            }
            else
            {
                //need to write
                Debug.LogError("This should never happen");
            }
            if (pathingRight)
            {
                finalPath.Enqueue(PathRight(pathTo));
            }
            else if (pathingLeft)
            {
                finalPath.Enqueue(PathLeft(pathTo));
            }
        }
        else if(pathingRight)
        {
            finalPath.Enqueue(PathRight(pathTo));
        }
        else if (pathingLeft)
        {
            finalPath.Enqueue(PathLeft(pathTo));
        }
        else
        {
            Debug.LogError("This should never happen");
        }
        /*
        else
        {
            finalX = pathFrom.x;
            finalY = pathFrom.y;
        }
        
        finalPath = new Vector2(finalX, finalY);
        */

        if(!finalPath.Any())
        {
            Debug.LogError("List is empty, this should never happen. Fix plz");
        }

        return finalPath;
    }

    private Vector2 GetMovementFromEdge(Vector2 edge)
    {
        var actualMovement = new Vector2(edge.x + 6f, edge.y);

        return actualMovement;
    }

    private Vector2 PathRight(Vector2 pathTo)
    {
        var finalX = pathTo.x - 4f;
        var finalY = pathTo.y;

        return new Vector2(finalX, finalY);
    }
    private Vector2 PathLeft(Vector2 pathTo)
    {
        var finalX = pathTo.x + 4f;
        var finalY = pathTo.y;

        return new Vector2(finalX, finalY);
    }
}