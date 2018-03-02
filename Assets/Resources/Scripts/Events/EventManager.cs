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
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (_eventManager == null)
                {
                    Debug.LogError("There is no Event Manager in the scene.");
                }
            }

            return _eventManager;
        }
    }

    private static EventManager _eventManager;

    private IDictionary<EventType, UnityEvent> _events;

    // Use this for initialization
    private void Awake()
    {
        _events = ((EventType[])Enum.GetValues(typeof(EventType)))
            .ToDictionary(eventType => eventType, eventType => new UnityEvent());
    }

    public void AddListener(EventType type, UnityAction action)
    {
        _events[type].AddListener(action);
    }

    public void RemoveListener(EventType type, UnityAction action)
    {
        _events[type].RemoveListener(action);
    }

    public void ExecuteEvent(EventType type)
    {
        _events[type].Invoke();
    }
}
