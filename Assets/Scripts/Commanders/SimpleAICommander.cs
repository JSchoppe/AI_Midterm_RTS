using UnityEngine; // TODO wrap Vector3. Script should be engine agnostic.

using AI_Midterm_RTS.EngineInterop;
using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Bases;

namespace AI_Midterm_RTS.Commanders
{
    /// <summary>
    /// Implements an AI pattern that spawns enemies from bases with
    /// the most enemy units nearby.
    /// </summary>
    public sealed class SimpleAICommander : Commander
    {
        #region Fields
        private readonly ITickProvider tickProvider;
        private float thoughtCounter;
        private float thoughtInterval;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new commander with the given team and tick provider.
        /// </summary>
        /// <param name="teamID">Controls how this AI's units attack other units.</param>
        /// <param name="tickProvider">Provides the tick implementation for tick routines.</param>
        public SimpleAICommander(byte teamID, ITickProvider tickProvider) : base(teamID)
        {
            // Subscribe thought to tick.
            this.tickProvider = tickProvider;
            tickProvider.Tick += AIThoughtTick;
            // Set default values.
            thoughtCounter = 0f;
            thoughtInterval = 1f;
            // Listen to stop ticking.
            AllBasesDefeated += OnAllBasesDefeated;
        }
        #endregion
        #region Properties
        /// <summary>
        /// How often this AI considers building a unit.
        /// </summary>
        public float ThoughtInterval
        {
            get => thoughtInterval;
            set
            {
                thoughtInterval = Mathf.Max(0f, value);
                thoughtCounter = 0f;
            }
        }
        #endregion
        #region AI Tick Routine
        private void AIThoughtTick(float deltaTime)
        {
            thoughtCounter += deltaTime;
            // This is a while to account for lag, since
            // we don't know if our Tick provider is fixed.
            while (thoughtCounter > thoughtInterval)
            {
                thoughtCounter -= thoughtInterval;
                TakeAction();
            }
        }
        private void TakeAction()
        {
            // Only deploy a unit if under the range.
            if (DeployedUnits.Count < MaxUnitCount)
            {
                // Calculate the average position of 
                // enemy units on the map.
                int enemyCount = 0;
                Vector3 averageEnemyPosition = default;
                foreach (Commander commander in AllCommanders)
                {
                    if (commander.TeamID != TeamID)
                    {
                        foreach (CombatActor unit in commander.DeployedUnits)
                        {
                            enemyCount++;
                            averageEnemyPosition += unit.Location;
                        }
                    }
                }
                if (enemyCount != 0)
                    averageEnemyPosition /= enemyCount;
                // Find the closest base that can deploy a unit.
                float closestSqrDistance = float.MaxValue;
                Factory closestBase = null;
                foreach (Factory unitBase in Bases)
                {
                    if (unitBase.IsCharged)
                    {
                        float sqrDistance =
                            Vector3.SqrMagnitude(unitBase.Location - averageEnemyPosition);
                        if (sqrDistance < closestSqrDistance)
                        {
                            closestBase = unitBase;
                            closestSqrDistance = sqrDistance;
                        }
                    }
                }
                // Spawn a unit at this base.
                if (closestBase != null)
                    closestBase.SpawnRandomInRadius(this);
            }
        }
        #endregion
        #region All Bases Defeated Listener
        private void OnAllBasesDefeated(Commander obj)
        {
            // Stop ticking; the AI can't do
            // anything anymore.
            tickProvider.Tick -= AIThoughtTick;
        }
        #endregion
    }
}
