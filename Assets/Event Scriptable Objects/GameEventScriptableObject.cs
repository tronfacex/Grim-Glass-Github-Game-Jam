using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event", menuName = "Scriptable Objects/Game Event")]
public class GameEventScriptableObject : ScriptableObject
{
    // How to create new events
    // Note: Create new assets in Unity by right clicking -> Scriptable Objects -> Game Event & naming it appropriately.
    // Then add a public GameEventScriptableObject to the script that will Raise the event. 
    // Lastly, drop the GameEventListener script on the GameObject(s) that will respond to the event 
    // and configure in the inspector. 
    // The method being called by the event must be public (since they will be fired by GameEventListener).

    /// <summary>
    /// The list of listeners that this event will notify if it is raised. 
    /// </summary>
    private readonly List<GameEventListener> eventListeners =
        new List<GameEventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
