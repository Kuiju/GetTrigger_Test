using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class EventTriggerManager : MonoBehaviour
{
    //注册EventTrigger事件（无参）
    public static void AddEventTrigger(EventTrigger eventTriggerArea, UnityAction eventFunction, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => eventFunction()); // you can capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTriggerArea.triggers.Add(entry);
    }

    //注册EventTrigger事件（string参数）
    public static void AddEventTrigger(EventTrigger eventTriggerArea, UnityAction<string> eventFunction, EventTriggerType triggerType, string eventParameter)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => eventFunction(eventParameter)); // you can capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTriggerArea.triggers.Add(entry);
    }
}
