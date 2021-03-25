using AI_Midterm_RTS.Bases;

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Base class for states that are focused on a target base.
    /// </summary>
    public abstract class TargetedBaseState : PeriodicEvaluatingState
    {
        #region Fields
        private readonly CombatActor actor;
        private Base target;
        #endregion
        #region Base Constructor
        public TargetedBaseState(CombatActor actor, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The current target.
        /// </summary>
        public Base Target
        {
            get => target;
            set
            {
                // Is this a new target?
                if (value != target)
                {
                    // If there was an old target, stop listening
                    // to their destroyed state.
                    if (target != null)
                        target.BaseDestroyed -= OnTargetDestroyed;
                    // Listen to the new target's destroyed state.
                    target = value;
                    target.BaseDestroyed += OnTargetDestroyed;
                }
            }
        }
        #endregion
        #region Target Defeated Listener
        private void OnTargetDestroyed(Base destroyedBase)
        {
            // The target is destroyed, what do we do now?
            target.BaseDestroyed -= OnTargetDestroyed;
            target = null;
            actor.EvaluateStateChange();
        }
        #endregion
    }
}
