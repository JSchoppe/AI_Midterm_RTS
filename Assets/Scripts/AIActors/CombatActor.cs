using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AICore;
using AI_Midterm_RTS.AICore.Distributions;
using AI_Midterm_RTS.Indicators;
using AI_Midterm_RTS.Navigation;
using AI_Midterm_RTS.Commanders;
using AI_Midterm_RTS.Bases;
using AI_Midterm_RTS.AIActors.States;

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
        private Transform transform;
        private Vector3 location;
        private float health;
        private float maxHealth;
        private float speedFactor;
        private float damageDealt;
        private IMeter healthMeter;
        private INavigator navigator;
        #endregion
        #region Constructors
        protected CombatActor(Transform transform, Dictionary<State, IState> states,
            float health = 100f, float maxHealth = 100f)
            : base(states)
        {
            this.transform = transform;
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
        /// Dictates the likeliness of state changes.
        /// </summary>
        public WeightedTable<State> StateTable { get; set; }
        /// <summary>
        /// The target actor of the current state (if applicable).
        /// </summary>
        public CombatActor TargetActor
        {
            get
            {
                // If the current state targets an actor
                // then return that actor.
                if (this[CurrentState] is TargetedActorState state)
                    return state.Target;
                else
                    return null;
            }
            set
            {
                // TODO this is kind of sloppy.
                // Apply the target to all relevent states.
                foreach (IState state in states.Values)
                    if (state is TargetedActorState targetState)
                        targetState.Target = value;
            }
        }
        /// <summary>
        /// The location of this actor. Setting this value
        /// directly will teleport the actor.
        /// </summary>
        public Vector3 Location
        {
            get => transform.position;
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
        public void EvaluateStateChange()
        {
            // Get some information about the nearby environment.
            List<Commander> enemies = GetOpposingCommanders();
            List<Commander> allies = new List<Commander> { GetCommander() };
            List<BaseDistancePair> nearEnemyBases = GetBasesByProximity(enemies);
            List<ActorDistancePair> nearEnemies = GetUnitsByProximity(enemies);
            List<BaseDistancePair> nearAlliedBases = GetBasesByProximity(allies);
            List<ActorDistancePair> nearAlliedActors = GetUnitsByProximity(allies);
            // Apply special adjustments to the weight table.
            // These mask out state that can't be achieved in
            // the current map context.
            WeightedTable<State> adjustedTable = StateTable.Clone();
            if (nearEnemies.Count == 0)
            {
                adjustedTable.SetCoefficient(State.AttackingUnit, 0f);
                adjustedTable.SetCoefficient(State.Taunting, 0f);
            }
            if (nearEnemyBases.Count == 0)
                adjustedTable.SetCoefficient(State.AttackingBuilding, 0f);
            if (nearAlliedBases.Count == 0)
                adjustedTable.SetCoefficient(State.DefendingBuilding, 0f);

            // Pull the next state based on considerations.
            State nextState = adjustedTable.Next();
            switch (nextState)
            {
                case State.AttackingBuilding:
                    // Attack the nearest base.
                    ((TargetedBaseState)this[nextState]).Target = nearEnemyBases[0].Base;
                    break;
                case State.AttackingUnit:
                    // Attack the nearest unit.
                    ((TargetedActorState)this[nextState]).Target = nearEnemies[0].Actor;
                    break;
                case State.DefendingBuilding:
                    // Defend the nearest base.
                    ((TargetedBaseState)this[nextState]).Target = nearAlliedBases[0].Base;
                    break;
                case State.Taunting:
                    // Taunt the nearest unit.
                    ((TargetedActorState)this[nextState]).Target = nearEnemies[0].Actor;
                    break;
            }
            CurrentState = nextState;
        }
        /// <summary>
        /// Iterates and returns the opponent commanders.
        /// </summary>
        /// <returns>A collection of the opponent commanders.</returns>
        public List<Commander> GetOpposingCommanders()
        {
            List<Commander> opponents = new List<Commander>();
            foreach (Commander commander in Commander.AllCommanders)
                if (commander.TeamID != TeamID)
                    opponents.Add(commander);
            return opponents;
        }
        /// <summary>
        /// Retrieves a commander for this TeamID.
        /// </summary>
        /// <returns>The first commander that is on this team.</returns>
        public Commander GetCommander()
        {
            // TODO this is bad. Make this a prop.
            foreach (Commander commander in Commander.AllCommanders)
                if (commander.TeamID == TeamID)
                    return commander;
            return default;
        }
        /// <summary>
        /// Retrieves a collection of opponents sorted by proximity.
        /// </summary>
        /// <param name="commanders">The commanders to check units in.</param>
        /// <returns>A collection of actor distance data with closest actors first.</returns>
        public List<ActorDistancePair> GetUnitsByProximity(List<Commander> commanders)
        {
            List<ActorDistancePair> opponents = new List<ActorDistancePair>();
            // Iterate through all combat actors.
            foreach (Commander commander in commanders)
            {
                foreach (CombatActor actor in commander.DeployedUnits)
                {
                    // Create a pair for this data.
                    ActorDistancePair newPair = new ActorDistancePair(
                        actor, Vector3.SqrMagnitude(actor.Location - Location));
                    // Sort it into the collection in place.
                    bool foundInsertionPoint = false;
                    for (int i = 0; i < opponents.Count; i++)
                    {
                        if (newPair.DistanceSquared < opponents[i].DistanceSquared)
                        {
                            opponents.Insert(i, newPair);
                            foundInsertionPoint = true;
                            break;
                        }
                    }
                    if (!foundInsertionPoint)
                        opponents.Add(newPair);
                }
            }
            // Return the distance ordered collection,
            // with closest actors at the front.
            return opponents;
        }
        /// <summary>
        /// Retrieves a collection of active bases sorted by proximity.
        /// </summary>
        /// <param name="commanders">The commanders to check bases in.</param>
        /// <returns>A collection of base distance data with closest bases first.</returns>
        public List<BaseDistancePair> GetBasesByProximity(List<Commander> commanders)
        {
            List<BaseDistancePair> bases = new List<BaseDistancePair>();
            // Iterate through all bases on given commanders.
            foreach (Commander commander in commanders)
            {
                foreach (Base unitBase in commander.Bases)
                {
                    // Create a pair for this data.
                    BaseDistancePair newPair = new BaseDistancePair(
                        unitBase, Vector3.SqrMagnitude(unitBase.Location - Location));
                    // Sort it into the collection in place.
                    bool foundInsertionPoint = false;
                    for (int i = 0; i < bases.Count; i++)
                    {
                        if (newPair.DistanceSquared < bases[i].DistanceSquared)
                        {
                            bases.Insert(i, newPair);
                            foundInsertionPoint = true;
                            break;
                        }
                    }
                    if (!foundInsertionPoint)
                        bases.Add(newPair);
                }
            }
            // Return the distance ordered collection,
            // with closest actors at the front.
            return bases;
        }
        // TODO document or clean up somehow.
        public sealed class ActorDistancePair
        {
            public CombatActor Actor { get; }
            public float DistanceSquared { get; }
            public ActorDistancePair(CombatActor actor, float distanceSquared)
            {
                Actor = actor;
                DistanceSquared = distanceSquared;
            }
        }
        public sealed class BaseDistancePair
        {
            public Base Base { get; }
            public float DistanceSquared { get; }
            public BaseDistancePair(Base theBase, float distanceSquared)
            {
                Base = theBase;
                DistanceSquared = distanceSquared;
            }
        }
        #endregion
    }
}
