using UnityEngine; // TODO wrap Mathf and Vector3. Script should be engine agnostic.

using AI_Midterm_RTS.UnityCore; // TODO instantiator should not be in Unity Core.
using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Commanders;
using AI_Midterm_RTS.Indicators;
using AI_Midterm_RTS.EngineInterop;
using AI_Midterm_RTS.EngineInterop.Unity; // TODO pass tick provider through constructor.
using AI_Midterm_RTS.Input;

namespace AI_Midterm_RTS.Bases
{
    /// <summary>
    /// A base that is able to produce units.
    /// </summary>
    public sealed class Factory : Base, ICommanderFocusable, ICommanderClickable
    {
        #region Fields
        private bool isCreatingUnit;
        private Commander creatingCommander;
        private CommanderCursor creatingCursor;
        private float lastInstantiationTime;
        private IMeter cooldownMeter;
        private IArrow placementArrow;
        private IRing placementRing;
        private ITickProvider tickProvider;
        private IToggleable buildPreview;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new factory with the given building attributes.
        /// Fills in factory attributes with default values.
        /// </summary>
        /// <param name="position">The position of the base in 3D space.</param>
        /// <param name="damageRadius">The radius in which attacks are considered damaging.</param>
        /// <param name="health">The starting health of the base.</param>
        public Factory(Vector3 position, float damageRadius, float health)
            : base(position, damageRadius, health)
        {
            lastInstantiationTime = 0f;
            isCreatingUnit = false;
            tickProvider = UnityTickService.GetProvider(UnityLoopType.Update);
        }
        #endregion
        #region Factory Properties
        /// <summary>
        /// The radius in which units can be instantiated from this base.
        /// </summary>
        public float OuterInstantiationRadius { get; set; }
        /// <summary>
        /// The minimum radius of instantiation, prevents
        /// units from spawning on top of the base.
        /// </summary>
        public float InnerInstantiationRadius { get; set; }
        /// <summary>
        /// The cooldown between instantiation.
        /// </summary>
        public float CooldownSeconds { get; set; }
        /// <summary>
        /// Whether this factory can currently deploy a unit.
        /// </summary>
        public bool IsCharged =>
            Time.time - lastInstantiationTime > CooldownSeconds;
        /// <summary>
        /// The mechanism that creates the combat actor.
        /// </summary>
        public IInstantiator<CombatActor> Instantiator { get; set; }
        /// <summary>
        /// The cooldown meter that displays the factory build cooldown.
        /// </summary>
        public IMeter CooldownMeter
        {
            get => cooldownMeter;
            set
            {
                if (value != cooldownMeter)
                {
                    cooldownMeter = value;
                    // Configure the new meter.
                    cooldownMeter.AutoHidesAtFill = true;
                    SetCooldownMeter();
                }
            }
        }
        /// <summary>
        /// The arrow that show where a unit will be placed.
        /// </summary>
        public IArrow PlacementArrow
        {
            get => placementArrow;
            set
            {
                if (value != placementArrow)
                {
                    // Hide the previously bound placement arrow.
                    if (placementArrow != null)
                    {
                        // Copy state over to the new arrow.
                        value.IsHidden = placementArrow.IsHidden;
                        value.Base = placementArrow.Base;
                        value.End = placementArrow.End;
                        // Hide the old linked arrow.
                        placementArrow.IsHidden = true;
                    }
                    placementArrow = value;
                }
            }
        }
        /// <summary>
        /// The ring that shows the bounds of unit instantiation.
        /// </summary>
        public IRing PlacementRing
        {
            get => placementRing;
            set
            {
                if (value != placementRing)
                {
                    // Hide the previously bound ring.
                    if (placementRing != null)
                    {
                        // Copy state over to the new ring.
                        value.IsHidden = placementRing.IsHidden;
                        value.Location = placementRing.Location;
                        value.Radius = placementRing.Radius;
                        // Hide the old linked ring.
                        placementRing.IsHidden = true;
                    }
                    placementRing = value;
                }
            }
        }
        /// <summary>
        /// The toggleable element that shows up
        /// when hovering to build.
        /// </summary>
        public IToggleable BuildPreview
        {
            get => buildPreview;
            set
            {
                if (value != buildPreview)
                {
                    // TODO better code here to handle runtime assignment.
                    buildPreview = value;
                    buildPreview.IsToggled = false;
                }
            }
        }
        #endregion
        #region Direct Spawning Methods
        /// <summary>
        /// Spawns a unit randomly within the spawn radius of the factory.
        /// </summary>
        public void SpawnRandomInRadius(Commander commander)
        {
            if (CanSpawn(commander))
            {
                // Get a random point to spawn in the base radius.
                float radius = Random.Range(InnerInstantiationRadius, OuterInstantiationRadius);
                float angle = Random.Range(0f, 2f * Mathf.PI);
                // Create the new actor and register it to the commander.
                CombatActor newActor = Instantiator.Instantiate();
                newActor.Location = new Vector3(
                    Location.x + Mathf.Sin(angle) * radius,
                    Location.y,
                    Location.z + Mathf.Cos(angle) * radius);
                commander.RegisterUnit(newActor);
            }
        }
        #endregion
        #region Click Drag Spawn Methods
        public void ClickEnter(CommanderCursor cursor, Commander commander, Vector3 startPosition)
        {
            if (CanSpawn(commander))
            {
                // Store the current state for creating a unit.
                isCreatingUnit = true;
                creatingCommander = commander;
                creatingCursor = cursor;
                // Start a routine for dragging arrow out
                // to place a unit on the map.
                PlacementArrow.IsHidden = false;
                PlacementRing.IsHidden = false;
                PlacementArrow.Base = Location;
                // TODO 0.1 magic number - make field.
                PlacementRing.Location = Location + Vector3.up * 0.1f;
                PlacementRing.Radius = OuterInstantiationRadius;
                // TODO start routine here.
                tickProvider.Tick += ClickedTick;
            }
        }
        private void ClickedTick(float deltaTime)
        {
            // Calculate the distance to the cursor relative
            // to the plane the factory sits on.
            Vector3 flatCursorPoint = new Vector3(
                creatingCursor.CurrentHitPosition.x,
                Location.y,
                creatingCursor.CurrentHitPosition.z);
            float distance = Vector3.Distance(flatCursorPoint, Location);
            // Bound the distance inside the limits.
            distance = Mathf.Clamp(distance, InnerInstantiationRadius, OuterInstantiationRadius);
            // Update the arrow renderer.
            // TODO 0.1 magic number - make field.
            PlacementArrow.End = Location +
                (flatCursorPoint - Location).normalized * distance
                + Vector3.up * 0.1f;
        }
        public void ClickExit(CommanderCursor cursor, Commander commander, Vector3 endPosition)
        {
            // Is this the completion of a placement operation?
            if (isCreatingUnit && commander == creatingCommander)
            {
                isCreatingUnit = false;
                lastInstantiationTime = Time.time;
                tickProvider.Tick += RechargingTick;
                tickProvider.Tick -= ClickedTick;
                // Hide the placement elements.
                PlacementArrow.IsHidden = true;
                PlacementRing.IsHidden = true;
                // Create the new actor and register it to the commander.
                CombatActor newActor = Instantiator.Instantiate();
                newActor.Location = PlacementArrow.End;
                commander.RegisterUnit(newActor);
            }
        }
        #endregion
        #region Focus Listeners
        public void FocusedEnter(CommanderCursor cursor, Commander commander)
        {
            if (commander.TeamID == TeamID)
                tickProvider.Tick += FocusedTick;
        }
        private void FocusedTick(float deltaTime)
        {
            // Are we currently charged?
            // If so display a preview of the unit to create.
            BuildPreview.IsToggled =
                Time.time - lastInstantiationTime > CooldownSeconds;
        }
        public void FocusedExit(CommanderCursor cursor, Commander commander)
        {
            if (commander.TeamID == TeamID)
            {
                tickProvider.Tick -= FocusedTick;
                BuildPreview.IsToggled = false;
            }
        }
        #endregion
        #region Recharging Tick Routine
        private void RechargingTick(float deltaTime)
        {
            // Update the cooldown meter.
            SetCooldownMeter();
            // If we are fully charged, then stop ticking.
            if (Time.time - lastInstantiationTime > CooldownSeconds)
                tickProvider.Tick -= RechargingTick;
        }
        #endregion
        #region Utility Functions
        private void SetCooldownMeter()
        {
            if (CooldownMeter != null)
            {
                // Calculate the current cooldown meter state.
                float charge = Time.time - lastInstantiationTime;
                if (charge > CooldownSeconds)
                    charge = CooldownSeconds;
                CooldownMeter.SetMeter(charge, CooldownSeconds);
            }
        }
        private bool CanSpawn(Commander commander)
        {
            // Ensure that all conditions are met to spawn a unit.
            return commander.TeamID == TeamID
                && commander.TotalUnitCount < commander.MaxUnitCount
                && Time.time - lastInstantiationTime > CooldownSeconds
                && !isCreatingUnit;
        }
        #endregion
    }
}
