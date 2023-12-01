using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // This script is attached to GameObjects to listen for GameEventScriptableObjects to be Raised
    // it then fires a Unity Event that is set in the inspector
    // See the GameOperator GameObject for an example

    [Tooltip("Event to register with.")]
    public GameEventScriptableObject Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
