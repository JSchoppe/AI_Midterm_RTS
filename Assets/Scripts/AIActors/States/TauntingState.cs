using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.Commanders;

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Implements a basic taunting state for all combat actors.
    /// </summary>
    public sealed class TauntingState : TargetedActorState
    {
        #region Fields
        private readonly CombatActor actor;
        private float repathInterval;
        private float repathTimer;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new taunting state with the given actor and intervals.
        /// </summary>
        /// <param name="actor">The actor that will be taunting.</param>
        /// <param name="repathInterval">Controls how often the unit repaths to distract the taunted unit.</param>
        /// <param name="evaluateInterval">How often the state should be re-evaluated.</param>
        public TauntingState(CombatActor actor, float repathInterval, float evaluateInterval)
            : base(actor, evaluateInterval)
        {
            this.actor = actor;
            RepathInterval = repathInterval;
        }
        #endregion
        #region Properties
        // TODO this should have get/set safeguards.
        /// <summary>
        /// Controls how far the taunting unit will try to redirect.
        /// </summary>
        public float RedirectionRadius { get; set; }
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
            // On the unit that we are taunting:
            // Mask out all other states, so they can only attack this enemy.
            Target.StateTable.SetCoefficient(CombatActor.State.AttackingBuilding, 0f);
            Target.StateTable.SetCoefficient(CombatActor.State.DefendingBuilding, 0f);
            Target.StateTable.SetCoefficient(CombatActor.State.Taunting, 0f);
            Target.StateTable.SetCoefficient(CombatActor.State.Traveling, 0f);
            // Chase this actor!
            Target.TargetActor = actor;
            Target.CurrentState = CombatActor.State.AttackingUnit;
            // Initialize the taunt navigation.
            FindTauntDirection();
        }
        public override void StateExited()
        {
            base.StateExited();
            // Unmask the actions from the taunted unit.
            if (Target != null)
                Target.StateTable.ResetCoefficients();
        }
        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            repathTimer += deltaTime;
            if (repathTimer > repathInterval)
            {
                repathTimer = 0f;
                FindTauntDirection();
            }
        }
        private void FindTauntDirection()
        {
            // Go in the opposite direction of the
            // nearest allied base.
            // TODO this is very inefficient.
            actor.Navigator.Target = actor.Location + 
                (actor.Location - actor.GetBasesByProximity(
                    new List<Commander>() { actor.GetCommander() })[0]
                .Base.Location).normalized * RedirectionRadius;
        }
        #endregion
    }
}
