using UnityEngine; // TODO wrap Mathf and Transform. Script should be engine agnostic.

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Implements a traveling state that utilizes INavigator.
    /// </summary>
    public sealed class TravelingState : PeriodicEvaluatingState
    {
        #region Fields
        private readonly CombatActor actor;
        private float repathInterval;
        private float repathTimer;
        private float destinationTolerance;
        private Transform target;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new traveling state with the given actor and intervals.
        /// </summary>
        /// <param name="actor">The actor to travel.</param>
        /// <param name="repathInterval">The period of time between repathing.</param>
        /// <param name="evaluateInterval">The period of time between state evaluation.</param>
        public TravelingState(CombatActor actor,
            float repathInterval, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
            RepathInterval = repathInterval;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The time in seconds between repathing calculations.
        /// </summary>
        public float RepathInterval
        {
            get => repathInterval;
            set
            {
                value = Mathf.Max(0f, value);
                // Has the repath interval changed?
                if (value != repathInterval)
                {
                    repathInterval = value;
                    repathTimer = 0f;
                }
            }
        }
        /// <summary>
        /// Controls the distance in meters before this actor
        /// stops traveling.
        /// </summary>
        public float DestinationTolerance
        {
            get => destinationTolerance;
            set
            {
                value = Mathf.Max(0f, value);
                if (value != destinationTolerance)
                {
                    destinationTolerance = value;
                    // Update the navigator.
                    actor.Navigator.DestinationTolerance
                        = destinationTolerance;
                }
            }
        }
        /// <summary>
        /// The target that the agent is traveling towards.
        /// </summary>
        public Transform Target
        {
            get => target;
            set
            {
                // Is this a new target?
                if (value != target)
                {
                    // If so hasten repathing to
                    // occur next tick.
                    target = value;
                    repathTimer = repathInterval;
                }
            }
        }
        #endregion
        #region State Implementation
        public override sealed void StateEntered()
        {
            base.StateEntered();
            repathTimer = 0f;
            // Initialize and listen to actor speed.
            actor.SpeedFactorChanged += OnSpeedFactorChanged;
            OnSpeedFactorChanged(actor.SpeedFactor);
            // Ensure proper navigator state.
            actor.Navigator.IsPaused = false;
            actor.Navigator.Target =
                Target ? Target.position : Vector3.zero;
        }
        public override sealed void StateExited()
        {
            base.StateExited();
            // Stop listening to actor speed.
            actor.SpeedFactorChanged -= OnSpeedFactorChanged;
        }
        public override sealed void Tick(float deltaTime)
        {
            // Every so often we ask the actor to
            // refocus its pathing.
            repathTimer += deltaTime;
            if (repathTimer > repathInterval)
            {
                repathTimer -= repathInterval;
                // Reset the target.
                actor.Navigator.Target =
                    Target ? Target.position : Vector3.zero;
            }
            // Call the base tick which might
            // invoke a state change.
            base.Tick(deltaTime);
        }
        #endregion
        #region Actor Listeners
        private void OnSpeedFactorChanged(float newSpeed)
        {
            // Update the navigator.
            actor.Navigator.Speed = newSpeed;
        }
        #endregion
    }
}
