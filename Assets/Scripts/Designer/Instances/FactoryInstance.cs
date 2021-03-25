using UnityEngine;

using AI_Midterm_RTS.EngineInterop.Unity;
using AI_Midterm_RTS.UnityCore;
using AI_Midterm_RTS.Bases;
using AI_Midterm_RTS.Commanders;
using AI_Midterm_RTS.Indicators.Unity;
using AI_Midterm_RTS.Input;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of a factory.
    /// </summary>
    public sealed class FactoryInstance : UnityEditorWrapper<Factory>,
        ICommanderFocusable, ICommanderClickable
    {
        #region Instance Field
        private Factory factory;
        #endregion
        #region Inspector Fields
        [Header("Factory References")]
        [Tooltip("The health meter for this factory.")]
        [SerializeField] private UnityMeter healthMeter = default;
        [Tooltip("The cooldown meter for this factory.")]
        [SerializeField] private UnityMeter cooldownMeter = default;
        [Tooltip("The arrow to draw when placing a unit.")]
        [SerializeField] private UnityArrow placementArrow = default;
        [Tooltip("The ring to draw when placing a unit.")]
        [SerializeField] private UnityRing placementRing = default;
        [Tooltip("Spawns the unit on the map.")]
        [SerializeField] private PrefabInstantiator unitInstantiator = default;
        [Tooltip("The renderer for the build preview.")]
        [SerializeField] private UnityRotatingMesh buildPreview = default;
        [Header("Factory Attributes")]
        [Tooltip("Identifies what team this base belongs to.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("How close do units have to be to damage this building?")]
        [SerializeField] private float damageRadius = 1f;
        [Tooltip("The starting health of this building.")]
        [SerializeField] private float health = 100f;
        [Tooltip("The delay for building units.")]
        [SerializeField] private float cooldownSeconds = 1f;
        [Tooltip("Controls how far units can be spawned from this base.")]
        [SerializeField] private float instantiationInnerRadius = 1f;
        [Tooltip("Controls how far units can be spawned from this base.")]
        [SerializeField] private float instantiationOuterRadius = 5f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Prevent invalid inspector values.
            health = Mathf.Max(0f, health);
            cooldownSeconds = Mathf.Max(0f, cooldownSeconds);
            damageRadius = Mathf.Max(0f, damageRadius);
            instantiationOuterRadius = Mathf.Max(0f, instantiationOuterRadius);
            instantiationInnerRadius = Mathf.Max(0f,
                Mathf.Min(instantiationOuterRadius, instantiationInnerRadius));
        }
        #endregion
        #region IEditorWrapper Implementation
        /// <summary>
        /// Returns the factory instance, initializing it if necessary.
        /// </summary>
        /// <returns>The wrapped factory instance.</returns>
        public override Factory Unwrap()
        {
            // Initialize this factory, if not already initialized.
            if (factory == null)
            {
                factory = new Factory(transform.position, damageRadius, health)
                {
                    // Set the factory attributes.
                    CooldownSeconds = cooldownSeconds,
                    InnerInstantiationRadius = instantiationInnerRadius,
                    OuterInstantiationRadius = instantiationOuterRadius,
                    TeamID = teamID,
                    // Set the factory references.
                    Instantiator = unitInstantiator,
                    HealthMeter = healthMeter,
                    CooldownMeter = cooldownMeter,
                    PlacementArrow = placementArrow,
                    PlacementRing = placementRing,
                    BuildPreview = buildPreview
                };
            }
            return factory;
        }
        #endregion
        #region ICommander Focus and Click Wrappers
        // TODO this is cringe.
        public void FocusedEnter(CommanderCursor cursor, Commander commander)
        {
            Unwrap().FocusedEnter(cursor, commander);
        }
        public void FocusedExit(CommanderCursor cursor, Commander commander)
        {
            Unwrap().FocusedExit(cursor, commander);
        }
        public void ClickEnter(CommanderCursor cursor, Commander commander, Vector3 startPosition)
        {
            Unwrap().ClickEnter(cursor, commander, startPosition);
        }
        public void ClickExit(CommanderCursor cursor, Commander commander, Vector3 endPosition)
        {
            Unwrap().ClickExit(cursor, commander, endPosition);
        }
        #endregion
    }
}
