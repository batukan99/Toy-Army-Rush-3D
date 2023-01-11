using System.Collections.Generic;
using UnityEngine.Events;
using EventType = Game.Core.Enums.EventType;
using Game.Core.Vehicle;
using Game.Core.Army;

namespace Game.Core.Events 
{
    public static class EventBase
    {
        private static readonly Dictionary<EventType, UnityEvent> Events = new Dictionary<EventType, UnityEvent>();
        private static readonly Dictionary<EventType, BoolEvent> BoolEvents = new Dictionary<EventType, BoolEvent>();
        private static readonly Dictionary<EventType, PieceEvent> PieceEvents = new Dictionary<EventType, PieceEvent>();
        private static readonly Dictionary<EventType, AllyAIEvent> AllyAIEvents = new Dictionary<EventType, AllyAIEvent>();

        private class BoolEvent : UnityEvent<bool> { }
        private class PieceEvent : UnityEvent<Piece> { }
        private class AllyAIEvent : UnityEvent<AllyAI> { }

        public static void StartListening(EventType eventType, UnityAction listener) 
        {
            UnityEvent thisEvent;

            if(Events.ContainsKey(eventType))
            {
                thisEvent = Events[eventType];
            }
            else
            {
                thisEvent = new UnityEvent();
                Events.Add(eventType, thisEvent);
            }

            thisEvent.AddListener(listener);
        }

        public static void StopListening(EventType eventType, UnityAction listener) 
        {
            if (Events.TryGetValue(eventType, out UnityEvent thisEvent)) 
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void NotifyListeners(EventType eventType) 
        {
            if (Events.TryGetValue(eventType, out UnityEvent thisEvent)) 
            {
                thisEvent.Invoke();
            }
        }


        //Boolean overloaded version
        public static void StartListening(EventType eventType, UnityAction<bool> listener) 
        {
            BoolEvent thisEvent;

            if(BoolEvents.ContainsKey(eventType))
            {
                thisEvent = BoolEvents[eventType];
            }
            else
            {
                thisEvent = new BoolEvent();
                BoolEvents.Add(eventType, thisEvent);
            }

            thisEvent.AddListener(listener);
        }
        //Boolean overloaded version
        public static void StopListening(EventType eventType, UnityAction<bool> listener) 
        {
            if (BoolEvents.TryGetValue(eventType, out BoolEvent thisEvent)) 
            {
                thisEvent.RemoveListener(listener);
            }
        }
        //Boolean overloaded version
        public static void NotifyListeners(EventType eventType, bool data) 
        {
            if (BoolEvents.TryGetValue(eventType, out BoolEvent thisEvent)) 
            {
                thisEvent.Invoke(data);
            }
        }

        //Piece overloaded version
        public static void StartListening(EventType eventType, UnityAction<Piece> listener) 
        {
            PieceEvent thisEvent;

            if(PieceEvents.ContainsKey(eventType))
            {
                thisEvent = PieceEvents[eventType];
            }
            else
            {
                thisEvent = new PieceEvent();
                PieceEvents.Add(eventType, thisEvent);
            }

            thisEvent.AddListener(listener);
        }
        //Piece overloaded version
        public static void StopListening(EventType eventType, UnityAction<Piece> listener) 
        {
            if (PieceEvents.TryGetValue(eventType, out PieceEvent thisEvent)) 
            {
                thisEvent.RemoveListener(listener);
            }
        }
        //Piece overloaded version
        public static void NotifyListeners(EventType eventType, Piece data) 
        {
            if (PieceEvents.TryGetValue(eventType, out PieceEvent thisEvent)) 
            {
                thisEvent.Invoke(data);
            }
        }

        //AllyAI overloaded version
        public static void StartListening(EventType eventType, UnityAction<AllyAI> listener) 
        {
            AllyAIEvent thisEvent;

            if(AllyAIEvents.ContainsKey(eventType))
            {
                thisEvent = AllyAIEvents[eventType];
            }
            else
            {
                thisEvent = new AllyAIEvent();
                AllyAIEvents.Add(eventType, thisEvent);
            }

            thisEvent.AddListener(listener);
        }
        //AllyAI overloaded version
        public static void StopListening(EventType eventType, UnityAction<AllyAI> listener) 
        {
            if (AllyAIEvents.TryGetValue(eventType, out AllyAIEvent thisEvent)) 
            {
                thisEvent.RemoveListener(listener);
            }
        }
        //AllyAI overloaded version
        public static void NotifyListeners(EventType eventType, AllyAI data) 
        {
            if (AllyAIEvents.TryGetValue(eventType, out AllyAIEvent thisEvent)) 
            {
                thisEvent.Invoke(data);
            }
        }
        

        
    }
}

