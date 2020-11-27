using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Lucine.Helpers
{
    /// <summary>
    /// This class aims to simplify event use
    /// It is a singleton that keep tracks for global event
    /// It contains a pool of event that can be accessed from everywhere
    /// But you can also use your own pool of events to make things less global
    /// See EventsPool for more information
    /// </summary>
    public class Events : Singleton<Events>
    {
        /// <summary>
        /// the pool of events
        /// </summary>
        private readonly EventsPool pool = new EventsPool();

        
        /// <summary>
        /// Get an event from its name
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        /// <returns></returns>
        public EventType Get<EventType>() where EventType : EventBase, new()
        {
            return pool.Get<EventType>();
        }
    }
    
    /// <summary>
    /// A pool of events
    /// Keep trace of list of event from their name 
    /// </summary>
    public class EventsPool
    {
        /// <summary>
        /// The pool definition
        /// </summary>
        private readonly Dictionary<string, EventBase> events = new Dictionary<string, EventBase>();

        /// <summary>
        /// Get an event from its name. It the event already exist give it, else create a new one and keep it with the given name
        /// </summary>
        /// <param name="eventName">Name of the event to return (may be created if not exist)</param>
        /// <returns></returns>
        public EventType Get<EventType>() where EventType : EventBase, new()
        {
            string typeName = typeof(EventType).ToString();
            if (events.TryGetValue(typeName, out var evt))
                return evt as EventType;

            evt = Activator.CreateInstance(typeof(EventType)) as EventBase;
            events.Add(typeName,evt);
            return evt as EventType;
        }
    }

    /// <summary>
    /// Base class for event
    /// </summary>
    public class EventBase
    {
        
    }
    
    /// <summary>
    /// Events with no parameter
    /// </summary>
    public class Event : EventBase
    {
        // The action to call when dispatching
        private Action callback;

        /// <summary>
        /// Add a listener to the event
        /// </summary>
        /// <param name="handler">the listening function</param>
        public void AddListener(Action handler)
        {
            callback += handler;
        }

        /// <summary>
        /// Remove a listener from event
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveListener(Action handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Call all listeners
        /// </summary>
        public void Dispatch()
        {
            callback?.Invoke();
        }
    }
    
    /// <summary>
    /// Event with one parameter of Type T
    /// Only one parameter event is created; if need more create a class/struct with these parameters and une the one parameter event
    /// </summary>
    /// <typeparam name="T">T is the type of the parameter for listener</typeparam>
    public class Event<T> : EventBase
    {
        // the event function
        private Action<T> callback;

        /// <summary>
        /// Add a listener with one parameter
        /// </summary>
        /// <param name="handler">The function with the parameter type T to add</param>
        public void AddListener(Action<T> handler)
        {
            callback += handler;
        }

        /// <summary>
        /// Remove a listener with one parameter
        /// </summary>
        /// <param name="handler">Listener</param>
        public void RemoveListener(Action<T> handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// Call all listener 
        /// </summary>
        /// <param name="param">The T type parameter for all listener</param>
        public void Dispatch(T param)
        {
            callback?.Invoke(param);
        }
    }
}