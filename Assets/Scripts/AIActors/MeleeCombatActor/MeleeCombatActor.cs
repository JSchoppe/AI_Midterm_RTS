using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors.MeleeCombatActor
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
        private MeleeCombatActor(Dictionary<State, IState> states)
            : base(states)
        {
            type = CombatActorType.Melee;
        }
        /// <summary>
        /// Creates a new melee combat actor with default values.
        /// </summary>
        /// <returns>The new instance of the melee combat actor.</returns>
        public static MeleeCombatActor MakeActor()
        {
            var states = new Dictionary<State, IState>();
            var actor = new MeleeCombatActor(states);


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
