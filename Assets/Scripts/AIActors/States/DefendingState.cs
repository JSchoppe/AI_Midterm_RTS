using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Implements the defending state for all unit types.
    /// </summary>
    public sealed class DefendingState : TargetedBaseState
    {
        #region Fields
        private readonly CombatActor actor;
        private float repathInterval;
        private float repathTimer;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new defending state with the given actor and interval.
        /// </summary>
        /// <param name="actor">The actor that will defend the base.</param>
        /// <param name="repathInterval">The amount of time between repathing.</param>
        /// <param name="evaluateInterval">The interval between state re-evalutation.</param>
        public DefendingState(CombatActor actor, float repathInterval, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
            RepathInterval = repathInterval;
        }
        #endregion
        #region Properties
        // TODO these should have get/set safeguards.
        /// <summary>
        /// Controls how closely the defender can swarm around the base.
        /// </summary>
        public float SwarmRadiusMin { get; set; }
        /// <summary>
        /// Controls how loosely the defender can swarm around the base.
        /// </summary>
        public float SwarmRadiusMax { get; set; }
        /// <summary>
        /// Sets the frequency that the defender will move to guard
        /// a difference side of the base.
        /// </summary>
        public float RepathInterval
        {
            get => repathInterval;
            set
            {
                repathInterval = Mathf.Max(0f, value);
            }
        }
        #endregion
        #region State Implementation
        public override void StateEntered()
        {
            base.StateEntered();
            // Reset timers.
            repathTimer = 0f;
            // Initialize defense swarming.
            actor.Navigator.DestinationTolerance = 0f;
            SetSwarmDestination();
        }
        public override void StateExited()
        {
            base.StateExited();
        }
        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            repathTimer += deltaTime;
            if (repathTimer > repathInterval)
            {
                repathTimer = 0f;
                SetSwarmDestination();
            }
        }
        private void SetSwarmDestination()
        {
            // Choose a random angle and distance.
            float angle = Random.value * Mathf.PI * 2f;
            float magnitude = Random.Range(SwarmRadiusMin, SwarmRadiusMax);
            // Retarget the navigator to the new
            // defending location.
            actor.Navigator.Target = Target.Location +
                new Vector3(
                    Mathf.Sin(angle) * magnitude,
                    0f,
                    Mathf.Cos(angle) * magnitude);
        }
        #endregion
    }
}
