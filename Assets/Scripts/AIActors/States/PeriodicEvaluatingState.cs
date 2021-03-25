using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Base class for states that re-evaluate state on an interval.
    /// </summary>
    public abstract class PeriodicEvaluatingState : IState, ITickable
    {
        #region Fields
        private readonly CombatActor actor;
        private float evaluateInterval;
        private float evaluateTimer;
        #endregion
        #region Base Constructor
        public PeriodicEvaluatingState(CombatActor actor, float evaluateInterval)
        {
            this.actor = actor;
            EvaluateInterval = evaluateInterval;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The time in seconds between reevaluating state.
        /// </summary>
        public float EvaluateInterval
        {
            get => evaluateInterval;
            set
            {
                value = Mathf.Max(0f, value);
                // Has the evaluate interval changed?
                if (value != evaluateInterval)
                {
                    evaluateInterval = value;
                    evaluateTimer = 0f;
                }
            }
        }
        #endregion
        #region Base State Implementation
        public virtual void StateEntered()
        {
            // Reset the evaluation timer.
            evaluateTimer = 0f;
        }
        public virtual void StateExited()
        {
            
        }
        public virtual void Tick(float deltaTime)
        {
            // Every so often we ask the actor to
            // consider changing state.
            evaluateTimer += deltaTime;
            if (evaluateTimer > evaluateInterval)
            {
                evaluateTimer -= evaluateInterval;
                actor.EvaluateStateChange();
            }
        }
        #endregion
    }
}
