using System;
using UnityEngine; // TODO wrap Vector3, script should be engine agnostic.

using AI_Midterm_RTS.Indicators;

namespace AI_Midterm_RTS.Bases
{
    /// <summary>
    /// A team owned base that can be attacked.
    /// </summary>
    public class Base
    {
        #region Fields
        private bool isDestroyed;
        private float health;
        private float maxHealth;
        private IMeter healthMeter;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new base with the given geographic and health properties.
        /// </summary>
        /// <param name="position">The position of the base in 3D space.</param>
        /// <param name="damageRadius">The radius in which attacks are considered damaging.</param>
        /// <param name="health">The starting health of the base.</param>
        public Base(Vector3 position, float damageRadius, float health)
        {
            Location = position;
            // Squared is exposed so that distance calculations
            // can be performed faster per AI instance.
            DamageRadiusSquared = damageRadius * damageRadius;
            // Set health and max health equal, assuming we start at max health.
            Health = health;
            MaxHealth = health;
        }
        #endregion
        #region Reference Properties
        /// <summary>
        /// The health meter attached to this base.
        /// </summary>
        public IMeter HealthMeter
        {
            get => healthMeter;
            set
            {
                if (value != healthMeter)
                {
                    healthMeter = value;
                    HealthMeter.SetMeter(health, maxHealth);
                }
            }
        }
        #endregion
        #region State Properties
        /// <summary>
        /// The team that owns this base.
        /// </summary>
        public byte TeamID { get; set; }
        /// <summary>
        /// The location of this base in 3D space.
        /// </summary>
        public Vector3 Location { get; private set; }
        /// <summary>
        /// The squared damage radius of this base.
        /// </summary>
        public float DamageRadiusSquared { get; private set; }
        /// <summary>
        /// Whether this base is currently destroyed.
        /// </summary>
        public bool IsDestroyed => isDestroyed;
        /// <summary>
        /// The current health of this base.
        /// </summary>
        public float Health
        {
            get => health;
            set
            {
                if (value != health && !IsDestroyed)
                {
                    // Check if this health change has
                    // caused the base to be destroyed.
                    if (value <= 0f)
                    {
                        isDestroyed = true;
                        // Notify any listeners that this
                        // base was destroyed.
                        BaseDestroyed?.Invoke(this);
                    }
                    else
                        health = value;
                    if (HealthMeter != null)
                        HealthMeter.SetMeter(health, maxHealth);
                }
            }
        }
        /// <summary>
        /// The current maximum health of this base.
        /// </summary>
        public float MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value != maxHealth)
                {
                    maxHealth = value;
                    if (HealthMeter != null)
                        HealthMeter.SetMeter(health, maxHealth);
                }
            }
        }
        #endregion
        #region State Changed Events
        /// <summary>
        /// Called once this base has been destroyed.
        /// </summary>
        public event Action<Base> BaseDestroyed;
        #endregion
    }
}
