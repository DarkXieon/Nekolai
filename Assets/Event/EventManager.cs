using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void TestEventMethod();

    //public event TestEventMethod TestEvent { add { TestEvent += value; } remove; }

    

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

    private Dictionary<EventType, UnityEvent> _eventListeners;

    // Use this for initialization
    void Awake()
    {
        _eventListeners = new Dictionary<EventType, UnityEvent>();

        new List<EventType>(Enum.GetValues(typeof(EventType)) as EventType[])
            .ForEach(eventType => _eventListeners.Add(eventType, new UnityEvent()));
    }

    public static void AddListener(EventType type, UnityAction action)
    {
        //_eventManager._eventListeners.Add(EventType.PLAYER_TURNS, new GenericUnityEvent<EntityJump>());
    }
}
