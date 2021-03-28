using System;
using System.Collections.Generic;

namespace AI_Midterm_RTS.AICore
{
    /// <summary>
    /// Base class for finite state machines.
    /// </summary>
    /// <typeparam name="TStateKey">The key used to identify the state.</typeparam>
    public abstract class StateMachine<TStateKey> : ITickable
    {
        #region State Fields
        protected readonly Dictionary<TStateKey, IState> states;
        private TStateKey currentState;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new state machine with the given states and default state.
        /// </summary>
        /// <param name="states">The states accessible to this machine.</param>
        public StateMachine(Dictionary<TStateKey, IState> states)
        {
            this.states = states;
        }
        #endregion
        #region State Events
        /// <summary>
        /// Called every time this machine changes state.
        /// Sends the new state and the machine.
        /// </summary>
        public event Action<StateMachine<TStateKey>, TStateKey> StateChanged;
        #endregion
        #region State Properties
        /// <summary>
        /// The state that the machine is currently in.
        /// </summary>
        public TStateKey CurrentState
        {
            get => currentState;
            set
            {
                // Is this a new state?
                if (!currentState.Equals(value))
                {
                    // Exit current state
                    states[currentState].StateExited();
                    currentState = value;
                    // Enter new state.
                    states[currentState].StateEntered();
                    // Notify listeners.
                    StateChanged?.Invoke(this, currentState);
                }
            }
        }
        #endregion
        #region Protected State Accessor
        /// <summary>
        /// Retrieves a state with the given state key.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <returns>The IStateMachine state object.</returns>
        protected IState this[TStateKey key] => states[key];
        #endregion
        #region Tick Routine
        /// <summary>
        /// Ticks the current state in the machine if applicable.
        /// </summary>
        /// <param name="deltaTime">The elapsed time in seconds.</param>
        public virtual void Tick(float deltaTime)
        {
            // Invoke the current state's tick if applicable.
            if (this[currentState] is ITickable state)
                state.Tick(deltaTime);
        }
        #endregion
    }
}
