namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Base class for states that are focused on a target actor.
    /// </summary>
    public abstract class TargetedActorState : PeriodicEvaluatingState
    {
        #region Fields
        private readonly CombatActor actor;
        private CombatActor target;
        #endregion
        #region Base Constructor
        public TargetedActorState(CombatActor actor, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The current target.
        /// </summary>
        public CombatActor Target
        {
            get => target;
            set
            {
                // Is this a new target?
                if (value != target)
                {
                    // If there was an old target, stop listening
                    // to their defeated state.
                    if (target != null)
                        target.ActorDefeated -= OnTargetDefeated;
                    // Listen to the new target's defeated state.
                    target = value;
                    target.ActorDefeated += OnTargetDefeated;
                }
            }
        }
        #endregion
        #region Target Defeated Listener
        private void OnTargetDefeated(CombatActor defeatedActor)
        {
            // The target is defeated, what do we do now?
            target.ActorDefeated -= OnTargetDefeated;
            target = null;
            actor.EvaluateStateChange();
        }
        #endregion
    }
}
