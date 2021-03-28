using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Mathf. Script should be engine agnostic.

using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Bases;

namespace AI_Midterm_RTS.Commanders
{
    /// <summary>
    /// Base class for all commanders on the map.
    /// </summary>
    public abstract class Commander
    {
        #region Commander Communication TODO
        // TODO bad
        static Commander()
        {
            AllCommanders = new List<Commander>();
        }
        /// <summary>
        /// Holds all commanders currently created on the map.
        /// </summary>
        public static List<Commander> AllCommanders { get; private set; }
        /// <summary>
        /// Clears out any commanders.
        /// </summary>
        public static void FlushCommanders()
        {
            AllCommanders = new List<Commander>();
        }
        #endregion
        #region Fields
        private int maxUnitCount;
        private int totalUnitCount;
        private Dictionary<CombatActorType, int> unitCountOfType;
        private List<CombatActor> deployedUnits;
        private List<Base> bases;
        #endregion
        #region Events
        /// <summary>
        /// Called once every time that the commander
        /// loses all bases. Passes the commander that
        /// lost all bases.
        /// </summary>
        public event Action<Commander> AllBasesDefeated;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes the base commander collections.
        /// </summary>
        public Commander(byte teamID)
        {
            TeamID = teamID;
            bases = new List<Base>();
            deployedUnits = new List<CombatActor>();
            // Initialize the unit count of each type
            // to contain entries for each actor type.
            unitCountOfType = new Dictionary<CombatActorType, int>();
            foreach (CombatActorType type in Enum.GetValues(typeof(CombatActorType)))
                unitCountOfType.Add(type, 0);
            // TODO bad
            AllCommanders.Add(this);
        }
        #endregion
        #region Properties
        /// <summary>
        /// The team ID of this commander.
        /// </summary>
        public byte TeamID { get; set; }
        /// <summary>
        /// The maximum number of units that this commander can deploy.
        /// </summary>
        public int MaxUnitCount
        {
            get => maxUnitCount;
            set
            {
                maxUnitCount = Mathf.Max(0, value);
            }
        }
        /// <summary>
        /// The total number of units that this commander has deployed.
        /// </summary>
        public int TotalUnitCount => totalUnitCount;
        /// <summary>
        /// Gets the total units of the given combat actor type.
        /// </summary>
        /// <param name="type">The type of actor.</param>
        /// <returns>The total number of deployed units of the type.</returns>
        public int TotalUnitsOfType(CombatActorType type)
            => unitCountOfType[type];
        /// <summary>
        /// The current combat units deployed by this commander.
        /// </summary>
        public List<CombatActor> DeployedUnits => deployedUnits;
        /// <summary>
        /// The current non-destroyed bases this commander has.
        /// </summary>
        public List<Base> Bases => bases;
        #endregion
        #region Register Units/Bases
        /// <summary>
        /// Adds a new unit to this commander's deployed units.
        /// </summary>
        /// <param name="actorToRegister">The actor to add.</param>
        public void RegisterUnit(CombatActor actorToRegister)
        {
            // Add the new units.
            deployedUnits.Add(actorToRegister);
            actorToRegister.TeamID = TeamID;
            // Listen to this new unit.
            actorToRegister.ActorDefeated += OnActorDefeated;
            // Update the unit counts.
            totalUnitCount++;
            unitCountOfType[actorToRegister.Type]++;
        }
        /// <summary>
        /// Registers the base to belong to this commander.
        /// </summary>
        /// <param name="baseToRegister">The base to register.</param>
        public void RegisterBase(Base baseToRegister)
        {
            // Add the base to be visible to other commanders.
            bases.Add(baseToRegister);
            baseToRegister.TeamID = TeamID;
            // Respond to the base being destroyed.
            baseToRegister.BaseDestroyed += OnBaseDestroyed;
        }
        #endregion
        #region Actor/Bases Defeated Listeners
        // This listener responds to when combat units are defeated.
        private void OnActorDefeated(CombatActor actor)
        {
#if DEBUG
            if (!deployedUnits.Contains(actor))
                Debug.LogError("Tried to remove undeployed unit!");
#endif
            // Remove the actor from collections.
            deployedUnits.Remove(actor);
            actor.ActorDefeated -= OnActorDefeated;
            // Update the unit counts.
            totalUnitCount--;
            unitCountOfType[actor.Type]--;
        }
        // This listener responds to when a base has been destroyed.
        private void OnBaseDestroyed(Base baseDestroyed)
        {
#if DEBUG
            if (!bases.Contains(baseDestroyed))
                Debug.LogError("Tried to remove unregistered base!");
#endif
            // Remove the base from the list of bases.
            // This means that AI will no longer see this base.
            bases.Remove(baseDestroyed);
            // If this has reduced the base count to zero,
            // broadcast that this commander can no longer
            // create units.
            if (bases.Count == 0)
            {

                AllBasesDefeated?.Invoke(this);
            }
        }
        #endregion
    }
}
