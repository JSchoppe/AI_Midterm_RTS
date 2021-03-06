using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;
using AI_Midterm_RTS.AIActors.States;
using AI_Midterm_RTS.AIActors.States.Melee;
using AI_Midterm_RTS.Commanders;

namespace AI_Midterm_RTS.AIActors
{
    /// <summary>
    /// A combat actor that uses melee attacks.
    /// </summary>
    public sealed class MeleeCombatActor : CombatActor
    {
        #region Fields
        private float range;
        private float attackDelay;
        private float attackDamage;
        #endregion
        private MeleeCombatActor(Transform transform, Dictionary<State, IState> states)
            : base(transform, states)
        {
            type = CombatActorType.Melee;
        }
        /// <summary>
        /// Creates a new melee combat actor with default values.
        /// </summary>
        /// <param name="transform">The transform to attach the actor to.</param>
        /// <returns>The new instance of the melee combat actor.</returns>
        public static MeleeCombatActor MakeActor(Transform transform)
        {
            var states = new Dictionary<State, IState>();
            var actor = new MeleeCombatActor(transform, states);
            // Initialize all states.
            // TODO all magic numbers should be serialized to designer.
            states.Add(State.Disabled, new DisabledState(actor));
            states.Add(State.Traveling, new TravelingState(actor, 20f, 1f));
            states.Add(State.AttackingUnit, new MeleeAttackState(actor, 0.4f, 1f));
            states.Add(State.AttackingBuilding, new MeleeAttackBuildingState(actor, 3f));
            states.Add(State.DefendingBuilding, new DefendingState(actor, 4f, 2f)
            {
                SwarmRadiusMin = 5f,
                SwarmRadiusMax = 7f
            });
            states.Add(State.Taunting, new TauntingState(actor, 5f, 3f)
            {
                RedirectionRadius = 6f
            });
            return actor;
        }
        #region Melee Actor Properties
        /// <summary>
        /// The current melee range of this unit.
        /// </summary>
        public float Range
        {
            get => range;
            set
            {
                // Range cannot be less than zero.
                value = Mathf.Max(value, 0f);
                if (value != range)
                {
                    range = value;
                    // Notify if range changed.
                    RangeChanged?.Invoke(range);
                }
            }
        }
        /// <summary>
        /// The current attack delay of this unit.
        /// </summary>
        public float AttackDelay
        {
            get => attackDelay;
            set
            {
                // Fire delay must be positive.
                value = Mathf.Max(value, float.Epsilon);
                if (value != attackDelay)
                {
                    attackDelay = value;
                    // Notify if delay changed.
                    AttackDelayChanged?.Invoke(attackDelay);
                }
            }
        }
        /// <summary>
        /// The current shot damage of this unit.
        /// </summary>
        public float AttackDamage
        {
            get => attackDamage;
            set
            {
                // Shot damage cannot be negative.
                value = Mathf.Max(value, 0f);
                if (value != attackDamage)
                {
                    attackDamage = value;
                    // Notify if shot damage changed.
                    AttackDamageChanged?.Invoke(attackDamage);
                }
            }
        }
        #endregion
        #region Properties Changed Events
        /// <summary>
        /// Called every time the unit range changes.
        /// </summary>
        public event Action<float> RangeChanged;
        /// <summary>
        /// Called every time the unit fire delay changes.
        /// </summary>
        public event Action<float> AttackDelayChanged;
        /// <summary>
        /// Called every time the unit shot damage changes.
        /// </summary>
        public event Action<float> AttackDamageChanged;
        #endregion
    }
}
