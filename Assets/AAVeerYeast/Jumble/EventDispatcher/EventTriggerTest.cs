using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

/* UGUI注册各类事件的方法
 * 测试时 随意找一个物体
 * 加入 EventTrigger组件
 * 加入 此脚本组件
 * 关联 一个输出用的TEXT
 */

public class EventTriggerTest : MonoBehaviour
{
    EventTrigger eventTrigger = null;

    [SerializeField]
    private Text textField = null;

    // Use this for initialization
    void Start()
    {
        eventTrigger = gameObject.GetComponent<EventTrigger>();
        AddEventTrigger(OnPointerClick, EventTriggerType.PointerClick);
        AddEventTrigger(OnPointerEnter, EventTriggerType.PointerEnter);
        AddEventTrigger(OnPointerExit, EventTriggerType.PointerExit);
    }

    #region TriggerEventsSetup

    private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action()); // you can capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger

        eventTrigger.triggers.Add(entry);
        //eventTrigger.delegates.Add(entry);
    }

    private void AddEventTrigger(UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        // Create a nee TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener((eventData) => action(eventData)); // you can capture and pass the event data to the listener

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        eventTrigger.triggers.Add(entry);
        //eventTrigger.delegates.Add(entry);
    }

    #endregion

    #region Callbacks

    private void OnPointerClick(BaseEventData data)
    {
        textField.text = "OnPointerClick " + data.selectedObject;
    }

    private void OnPointerEnter()
    {
        textField.text = "OnPointerEnter";
    }

    private void OnPointerExit()
    {
        textField.text = "OnPointerExit";
    }

    #endregion

}
