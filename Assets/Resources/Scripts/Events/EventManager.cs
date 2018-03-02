using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance
    {
        get
        {
            if (_eventManager == null)
            {
                _eventManager = FindObjectOfType<EventManager>();

                if (_eventManager == null)
                {
                    Debug.LogError("There is no Event Manager in the scene.");
                }
            }

            return _eventManager;
        }
    }

    private static EventManager _eventManager;

    private GameObject _defaultEventObject;

    private IDictionary<EventType, Dictionary<GameObject, UnityEvent>> _events;

    // Use this for initialization
    private void Awake()
    {
        //Debug.Log("called");

        //_eventManager = this.GetComponent<EventManager>();
        
        _defaultEventObject = new GameObject();

        _events = ((EventType[])Enum.GetValues(typeof(EventType)))
            .ToDictionary(eventType => eventType, eventType => new Dictionary<GameObject, UnityEvent>());
    }

    public void AddListener(EventType type, UnityAction action)
    {
        this.AddObjectSpecificListener(type, action, this._defaultEventObject);
    }

    public void AddObjectSpecificListener(EventType type, UnityAction action, GameObject specificListener)
    {
        UnityEvent found;

        Debug.Assert(_events != null);

        if(!_events[type].TryGetValue(specificListener, out found))
        {
            found = new UnityEvent();

            _events[type].Add(specificListener, found);
        }

        found.AddListener(action);
    }

    public void RemoveListener(EventType type, UnityAction action)
    {
        this.RemoveObjectSpecificListener(type, action, this._defaultEventObject);
    }

    public void RemoveObjectSpecificListener(EventType type, UnityAction action, GameObject specificListener)
    {
        UnityEvent found;

        if (_events[type].TryGetValue(specificListener, out found))
        {
            found.RemoveListener(action);
        }
        else
        {
            Debug.LogError("No key found for event removal");
        }
    }

    public void ExecuteEvent(EventType type)
    {
        this.ExecuteObjectSpecificEvent(type, _defaultEventObject);
    }

    public void ExecuteObjectSpecificEvent(EventType type, GameObject specificListener)
    {
        UnityEvent found;

        if (_events[type].TryGetValue(specificListener, out found))
        {
            found.Invoke();
        }
        else
        {
            Debug.LogError("No key found for event invoke");
        }
    }
}
