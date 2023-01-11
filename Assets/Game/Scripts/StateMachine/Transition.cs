using System;

namespace Game.StateMachines 
{
    public class Transition
        {
            public Func<bool> Condition {get; }
            public State To { get; }

            public Transition(State to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
}
   