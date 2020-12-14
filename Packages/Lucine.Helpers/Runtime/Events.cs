using System;
using System.Collections.Generic;

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
        /// Get an event from its type
        /// </summary>
        /// <returns></returns>
        public EventType TypeOf<EventType>() where EventType : EventBase, new()
        {
            return pool.TypeOf<EventType>();
        }
    }
    
    /// <summary>
    /// A pool of events
    /// Keep trace of list of event from their type 
    /// </summary>
    public class EventsPool
    {
        /// <summary>
        /// The pool definition
        /// </summary>
        private readonly Dictionary<string, EventBase> m_Events = new Dictionary<string, EventBase>();

        /// <summary>
        /// Get an event from its type. It the event already exist give it, else create a new one and keep it with the given name
        /// </summary>
        /// <returns>The found event or created event if not exists</returns>
        public EventType TypeOf<EventType>() where EventType : EventBase, new()
        {
            // il the EventType name is found in pool return it
            string typeName = typeof(EventType).ToString();
            if (m_Events.TryGetValue(typeName, out var evt))
                return evt as EventType;

            // else created a new eventtype and store it before returning it
            evt = Activator.CreateInstance(typeof(EventType)) as EventBase;
            m_Events.Add(typeName,evt);
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
        private Action m_Callback;

        /// <summary>
        /// Add a listener to the event
        /// </summary>
        /// <param name="handler">the listening function</param>
        public void AddListener(Action handler)
        {
            m_Callback += handler;
        }

        /// <summary>
        /// Remove a listener from event
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveListener(Action handler)
        {
            m_Callback -= handler;
        }

        /// <summary>
        /// Call all listeners
        /// </summary>
        public void Dispatch()
        {
            m_Callback?.Invoke();
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
        private Action<T> m_Callback;

        /// <summary>
        /// Add a listener with one parameter
        /// </summary>
        /// <param name="handler">The function with the parameter type T to add</param>
        public void AddListener(Action<T> handler)
        {
            m_Callback += handler;
        }

        /// <summary>
        /// Remove a listener with one parameter
        /// </summary>
        /// <param name="handler">Listener</param>
        public void RemoveListener(Action<T> handler)
        {
            m_Callback -= handler;
        }

        /// <summary>
        /// Call all listener 
        /// </summary>
        /// <param name="param">The T type parameter for all listener</param>
        public void Dispatch(T param)
        {
            m_Callback?.Invoke(param);
        }
    }
}