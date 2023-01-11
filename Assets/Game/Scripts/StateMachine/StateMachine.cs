using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StateMachines 
{
    public class ArmyStateMachine
    {
        public State CurrentState {get; private set;}
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();
        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        public void LogicUpdate()
        {
            var transition = GetTransition();
            if (transition != null) {
                ChangeState(transition.To);
                
            }
      
            CurrentState?.LogicUpdate();
        }

        public void ChangeState(State newState) 
        {
            if (newState == CurrentState)
                return;

            CurrentState?.Exit();
            CurrentState = newState;

            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            CurrentState.Enter();
        }

        public void AddTransition(State from, State to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }
      
            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(State state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
            Debug.Log(_anyTransitions);
        }

        private Transition GetTransition()
        {
            foreach(var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;
      
            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }

}
