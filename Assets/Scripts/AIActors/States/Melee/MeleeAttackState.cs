using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

namespace AI_Midterm_RTS.AIActors.States.Melee
{
    /// <summary>
    /// The attack state specialized for the melee actor.
    /// </summary>
    public sealed class MeleeAttackState : TargetedActorState
    {
        #region Fields
        private readonly MeleeCombatActor actor;
        private float attackTimer;
        private float repathTimer;
        private float repathInterval;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new melee attack state with the given actor and interval.
        /// </summary>
        /// <param name="actor">The melee combat actor.</param>
        /// <param name="repathInterval">The time in seconds between repathing to attack.</param>
        /// <param name="evaluateInterval">The time in seconds between state change evaluation.</param>
        public MeleeAttackState(MeleeCombatActor actor, 
            float repathInterval, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
            RepathInterval = repathInterval;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Sets the frequency that repathing is done.
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
            attackTimer = 0f;
            repathTimer = 0f;
            // We want to get as close to the target as possible.
            actor.Navigator.DestinationTolerance = 0f;
            actor.Navigator.Target = Target.Location;
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
                // Reorient targeting.
                repathTimer = 0f;
                actor.Navigator.Target = Target.Location;
            }
            // Check attack timer.
            attackTimer += deltaTime;
            if (attackTimer > actor.AttackDelay)
            {
                attackTimer -= actor.AttackDelay;
                // Attempt to attack.
                if ((Target.Location - actor.Location).sqrMagnitude
                    < actor.Range * actor.Range)
                {
                    // Deal damage if within range of the target.
                    Target.Health -= actor.AttackDamage;
                    actor.DamageDealt += actor.AttackDamage;
                }
            }
        }
        #endregion
    }
}
