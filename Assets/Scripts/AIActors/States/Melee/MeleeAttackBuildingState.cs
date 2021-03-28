namespace AI_Midterm_RTS.AIActors.States.Melee
{
    // TODO this seems redundant/very similar to normal attack state.
    // Maybe generalize an IAttackable interface.
    /// <summary>
    /// Implements the building attacking state for melee actors.
    /// </summary>
    public sealed class MeleeAttackBuildingState : TargetedBaseState
    {
        #region Fields
        private readonly MeleeCombatActor actor;
        private float attackTimer;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new melee attack building state with the given actor and interval.
        /// </summary>
        /// <param name="actor">The actor to attack buildings.</param>
        /// <param name="evaluateInterval">The interval to re-evaluate state.</param>
        public MeleeAttackBuildingState(MeleeCombatActor actor, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
        }
        #endregion
        #region State Implementation
        public override void StateEntered()
        {
            base.StateEntered();
            // Reset timers.
            attackTimer = 0f;
            // We want to get as close to the base as possible.
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
            // Check attack timer.
            attackTimer += deltaTime;
            if (attackTimer > actor.AttackDelay)
            {
                attackTimer -= actor.AttackDelay;
                // Attempt to attack.
                if ((Target.Location - actor.Location).sqrMagnitude
                    < actor.Range * actor.Range + Target.DamageRadiusSquared)
                {
                    // Deal damage if within range of the base.
                    Target.Health -= actor.AttackDamage;
                    actor.DamageDealt += actor.AttackDamage;
                }
            }
        }
        #endregion
    }
}
