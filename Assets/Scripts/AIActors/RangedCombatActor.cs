using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors
{
    /// <summary>
    /// A combat actor that uses ranged attacks.
    /// </summary>
    public sealed class RangedCombatActor : CombatActor
    {
        #region Fields
        private float range;
        private float fireDelay;
        private float shotSpeed;
        private float shotDamage;
        #endregion
        private RangedCombatActor(Transform transform, Dictionary<State, IState> states)
            : base(transform, states)
        {
            type = CombatActorType.Ranged;
        }
        /// <summary>
        /// Creates a new ranged combat actor with default values.
        /// </summary>
        /// <param name="transform">The transform to attach the actor to.</param>
        /// <returns>The new instance of the ranged combat actor.</returns>
        public static RangedCombatActor MakeActor(Transform transform)
        {
            var states = new Dictionary<State, IState>();
            var actor = new RangedCombatActor(transform, states);


            return actor;
        }
        #region Ranged Actor Properties
        /// <summary>
        /// The current range of this unit.
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
        /// The current fire delay of this unit.
        /// </summary>
        public float FireDelay
        {
            get => fireDelay;
            set
            {
                // Fire delay must be positive.
                value = Mathf.Max(value, float.Epsilon);
                if (value != fireDelay)
                {
                    fireDelay = value;
                    // Notify if delay changed.
                    FireDelayChanged?.Invoke(fireDelay);
                }
            }
        }
        /// <summary>
        /// The current shot speed of this unit.
        /// </summary>
        public float ShotSpeed
        {
            get => shotSpeed;
            set
            {
                // Fire speed must be positive.
                value = Mathf.Max(value, float.Epsilon);
                if (value != shotSpeed)
                {
                    shotSpeed = value;
                    // Notify if shot speed changed.
                    ShotSpeedChanged?.Invoke(shotSpeed);
                }
            }
        }
        /// <summary>
        /// The current shot damage of this unit.
        /// </summary>
        public float ShotDamage
        {
            get => shotDamage;
            set
            {
                // Shot damage cannot be negative.
                value = Mathf.Max(value, 0f);
                if (value != shotDamage)
                {
                    shotSpeed = value;
                    // Notify if shot damage changed.
                    ShotDamageChanged?.Invoke(shotDamage);
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
        public event Action<float> FireDelayChanged;
        /// <summary>
        /// Called every time the unit shot speed changes.
        /// </summary>
        public event Action<float> ShotSpeedChanged;
        /// <summary>
        /// Called every time the unit shot damage changes.
        /// </summary>
        public event Action<float> ShotDamageChanged;
        #endregion
    }
}
