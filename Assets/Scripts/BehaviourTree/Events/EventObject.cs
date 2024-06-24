using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class EventObject : ScriptableObject
{
    public List<System.Action<bool>> subscribers = new List<System.Action<bool>>();

    public void Subscribe(System.Action<bool> subscriber)
    {
        if(!subscribers.Contains(subscriber))
        {
            subscribers.Add(subscriber);
        }
    }

    public void Invoke(bool isTrue)
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i](isTrue);
        }
    }
}
