using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors
{
    /// <summary>
    /// A combat actor that deals area of effect attacks.
    /// </summary>
    public sealed class AOECombatActor : CombatActor
    {
        #region Fields
        private float attackDelay;
        private float attackRadius;
        private float attackDamage;
        #endregion
        private AOECombatActor(Transform transform, Dictionary<State, IState> states)
            : base(transform, states)
        {
            type = CombatActorType.AOE;
        }
        /// <summary>
        /// Creates a new jousting combat actor with default values.
        /// </summary>
        /// <param name="transform">The transform to attach the actor to.</param>
        /// <returns>The new instance of the jousting combat actor.</returns>
        public static AOECombatActor MakeActor(Transform transform)
        {
            var states = new Dictionary<State, IState>();
            var actor = new AOECombatActor(transform, states);


            return actor;
        }
        #region Ranged Actor Properties
        /// <summary>
        /// The current attack delay of this unit.
        /// </summary>
        public float AttackDelay
        {
            get => attackDelay;
            set
            {
                // Delay must be greater than zero.
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
        /// The current attack radius of this unit.
        /// </summary>
        public float AttackRadius
        {
            get => attackRadius;
            set
            {
                // Radius cannot be negative.
                value = Mathf.Max(value, 0f);
                if (value != attackRadius)
                {
                    attackRadius = value;
                    // Notify if radius changed.
                    AttackRadiusChanged?.Invoke(attackRadius);
                }
            }
        }
        /// <summary>
        /// The current area of effect damage of this unit.
        /// </summary>
        public float AttackDamage
        {
            get => attackDamage;
            set
            {
                // Damage cannot be negative.
                value = Mathf.Max(value, 0f);
                if (value != attackDamage)
                {
                    attackDamage = value;
                    // Notify if damage changed.
                    AttackDamageChanged?.Invoke(attackDamage);
                }
            }
        }
        #endregion
        #region Properties Changed Events
        /// <summary>
        /// Called every time the unit attack delay changes.
        /// </summary>
        public event Action<float> AttackDelayChanged;
        /// <summary>
        /// Called every time the unit attack radius changes.
        /// </summary>
        public event Action<float> AttackRadiusChanged;
        /// <summary>
        /// Called every time the unit attack damage changes.
        /// </summary>
        public event Action<float> AttackDamageChanged;
        #endregion
    }
}
