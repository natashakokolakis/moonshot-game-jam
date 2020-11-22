using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventManagerNorth : MonoBehaviour
{
    private Dictionary <string, UnityEvent> eventDictionary;

    private static EventManagerNorth eventManagerNorth;

    public static EventManagerNorth instance
    {
        get
        {
            if (!eventManagerNorth)
            {
                eventManagerNorth = FindObjectOfType(typeof(EventManagerNorth)) as EventManagerNorth;
            
                if (!eventManagerNorth)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManagerNorth.Init();
                }
            }

            return eventManagerNorth;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

    }

    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        } else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        if (eventManagerNorth == null) return;
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

}
