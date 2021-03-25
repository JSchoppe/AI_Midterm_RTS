using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors.JoustingCombatActor
{
    /// <summary>
    /// A combat actor that employs jousting attacks.
    /// </summary>
    public sealed class JoustingCombatActor : CombatActor
    {
        #region Fields
        private float joustingRange;
        private float joustingSpeed;
        private float joustingDamage;
        #endregion
        private JoustingCombatActor(Dictionary<State, IState> states)
            : base(states)
        {
            type = CombatActorType.Jousting;
        }
        /// <summary>
        /// Creates a new jousting combat actor with default values.
        /// </summary>
        /// <returns>The new instance of the jousting combat actor.</returns>
        public static JoustingCombatActor MakeActor()
        {
            var states = new Dictionary<State, IState>();
            var actor = new JoustingCombatActor(states);


            return actor;
        }
        #region Ranged Actor Properties
        /// <summary>
        /// The current jousting range of this unit.
        /// </summary>
        public float JoustingRange
        {
            get => joustingRange;
            set
            {
                // Range cannot be less than zero.
                value = Mathf.Max(value, 0f);
                if (value != joustingRange)
                {
                    joustingRange = value;
                    // Notify if range changed.
                    JoustingRangeChanged?.Invoke(joustingRange);
                }
            }
        }
        /// <summary>
        /// The current jousting speed of this unit.
        /// </summary>
        public float JoustingSpeed
        {
            get => joustingSpeed;
            set
            {
                // Jousting speed must be positive.
                value = Mathf.Max(value, float.Epsilon);
                if (value != joustingSpeed)
                {
                    joustingSpeed = value;
                    // Notify if speed changed.
                    JoustingSpeedChanged?.Invoke(joustingSpeed);
                }
            }
        }
        /// <summary>
        /// The current jousting damage of this unit.
        /// </summary>
        public float JoustingDamage
        {
            get => joustingDamage;
            set
            {
                // Damage cannot be negative.
                value = Mathf.Max(value, 0f);
                if (value != joustingDamage)
                {
                    joustingDamage = value;
                    // Notify if damage changed.
                    JoustingDamageChanged?.Invoke(joustingDamage);
                }
            }
        }
        #endregion
        #region Properties Changed Events
        /// <summary>
        /// Called every time the unit jousting range changes.
        /// </summary>
        public event Action<float> JoustingRangeChanged;
        /// <summary>
        /// Called every time the unit jousting speed changes.
        /// </summary>
        public event Action<float> JoustingSpeedChanged;
        /// <summary>
        /// Called every time the unit jousting damage changes.
        /// </summary>
        public event Action<float> JoustingDamageChanged;
        #endregion
    }
}
