using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;
using AI_Midterm_RTS.Indicators;
using AI_Midterm_RTS.Navigation;
using AI_Midterm_RTS.Commanders;

namespace AI_Midterm_RTS.AIActors
{
    /// <summary>
    /// Base class for combat actor state machines.
    /// </summary>
    public abstract class CombatActor : StateMachine<CombatActor.State>
    {
        #region Properties Changed Events
        /// <summary>
        /// Called every time the unit health changes.
        /// </summary>
        public event Action<float> HealthChanged;
        /// <summary>
        /// Called every time the unit max health changes.
        /// </summary>
        public event Action<float> MaxHealthChanged;
        /// <summary>
        /// Called every time the total damage dealth changes.
        /// </summary>
        public event Action<float> DamageDealtChanged;
        /// <summary>
        /// Called every time the unit speed factor changes.
        /// </summary>
        public event Action<float> SpeedFactorChanged;
        /// <summary>
        /// Called once when this actor has been defeated.
        /// </summary>
        public event Action<CombatActor> ActorDefeated;
        #endregion
        #region State Definition
        /// <summary>
        /// Defines the state for combat units.
        /// </summary>
        public enum State : byte
        {
            /// <summary>
            /// The unit is not active.
            /// </summary>
            Disabled,
            /// <summary>
            /// The unit is traveling to a point of interest.
            /// </summary>
            Traveling,
            /// <summary>
            /// The unit is attacking another target unit.
            /// </summary>
            AttackingUnit,
            /// <summary>
            /// The unit is attacking a target building.
            /// </summary>
            AttackingBuilding,
            /// <summary>
            /// The unit is defending a targeted building.
            /// </summary>
            DefendingBuilding,
            /// <summary>
            /// The unit is distracting another unit.
            /// </summary>
            Taunting
        }
        #endregion
        #region Fields
        protected CombatActorType type;
        private Vector3 location;
        private float health;
        private float maxHealth;
        private float speedFactor;
        private float damageDealt;
        private IMeter healthMeter;
        private INavigator navigator;
        #endregion
        #region Constructors
        protected CombatActor(Dictionary<State, IState> states,
            float health = 100f, float maxHealth = 100f)
            : base(states)
        {
            // Initialize property values.
            this.health = health;
            this.maxHealth = maxHealth;
            this.speedFactor = 1f;
            this.damageDealt = 0f;
        }
        #endregion
        #region Actor Properties
        /// <summary>
        /// The team this actor is associated with.
        /// </summary>
        public byte TeamID { get; set; }
        /// <summary>
        /// The type of actor this is.
        /// </summary>
        public CombatActorType Type => type;
        /// <summary>
        /// The location of this actor. Setting this value
        /// directly will teleport the actor.
        /// </summary>
        public Vector3 Location
        {
            get => location;
            set
            {
                if (value != location)
                {
                    location = value;
                    // Teleport directly to the new location.
                    Navigator.Teleport(location);
                }
            }
        }
        /// <summary>
        /// The current health points assiged to this unit.
        /// </summary>
        public float Health
        {
            get => health;
            set
            {
                if (value != health)
                {
                    // Don't let health drop below zero.
                    // If it is at zero change to disabled state.
                    health = Mathf.Clamp(value, 0f, maxHealth);
                    if (health == 0f && CurrentState != State.Disabled)
                    {
                        CurrentState = State.Disabled;
                        // Notify listeners that this actor
                        // has been defeated.
                        ActorDefeated?.Invoke(this);
                    }
                    HealthChanged?.Invoke(health);
                    // Update the health meter.
                    OnMeterValueChanged();
                }
            }
        }
        /// <summary>
        /// The maximum health value for this unit.
        /// </summary>
        public float MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value != maxHealth)
                {
                    maxHealth = Mathf.Max(value, 0f);
                    MaxHealthChanged?.Invoke(maxHealth);
                    // Check to see if this changes the current health.
                    Health = Mathf.Min(Health, maxHealth);
                    // Update the health meter.
                    OnMeterValueChanged();
                }
            }
        }
        /// <summary>
        /// The coefficient to augment this unit's speed.
        /// </summary>
        public float SpeedFactor
        {
            get => speedFactor;
            set
            {
                if (value != speedFactor)
                {
                    speedFactor = value;
                    SpeedFactorChanged?.Invoke(speedFactor);
                }
            }
        }
        /// <summary>
        /// The total damage that this unit has dealt.
        /// </summary>
        public float DamageDealt
        {
            get => damageDealt;
            set
            {
                if (value != damageDealt)
                {
                    damageDealt = value;
                    DamageDealtChanged?.Invoke(damageDealt);
                }
            }
        }
        #endregion
        #region Actor References
        /// <summary>
        /// The health meter for this actor.
        /// </summary>
        public IMeter HealthMeter
        {
            get => healthMeter;
            set
            {
                if (value != healthMeter)
                {
                    healthMeter = value;
                    // Initialize the new meter.
                    OnMeterValueChanged();
                }
            }
        }
        /// <summary>
        /// The navigator for this actor.
        /// </summary>
        public INavigator Navigator
        {
            get => navigator;
            set
            {
                if (value != navigator)
                    navigator = value;
            }
        }
        #endregion
        #region Reference Listeners
        private void OnMeterValueChanged()
        {
            if (healthMeter != null)
                healthMeter.SetMeter(health, maxHealth);
        }
        #endregion
        #region Common Methods
        /// <summary>
        /// Called whenever a state wants the actor to consider
        /// changing to a different state.
        /// </summary>
        public virtual void EvaluateStateChange()
        {

        }
        /// <summary>
        /// Iterates and returns the opponent commanders.
        /// </summary>
        /// <returns>A collection of the opponent commanders.</returns>
        protected List<Commander> GetOpposingCommanders()
        {
            List<Commander> opponents = new List<Commander>();
            foreach (Commander commander in Commander.AllCommanders)
                if (commander.TeamID != TeamID)
                    opponents.Add(commander);
            return opponents;
        }
        #endregion
    }
}
