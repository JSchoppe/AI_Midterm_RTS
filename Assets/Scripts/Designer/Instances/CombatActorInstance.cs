using UnityEngine;

using AI_Midterm_RTS.EngineInterop.Unity;
using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Indicators.Unity;
using AI_Midterm_RTS.Navigation.Unity;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// Base class for scene instances deriving from the combat actor.
    /// </summary>
    public abstract class CombatActorInstance : UnityEditorWrapper<CombatActor>
    {
        #region Inspector Fields
        [Header("Combat Actor References")]
        [Tooltip("The meter used to display health for this unit.")]
        [SerializeField] protected UnityMeter healthMeter = default;
        [Tooltip("The navigation used by this actor.")]
        [SerializeField] protected UnityNavigator navigator = default;
        [Tooltip("The base weighted table for this actor.")]
        [SerializeField] protected CombatActorStateWeightedTableInstance table = default;
        [Header("Combat Actor Attributes")]
        [Tooltip("The starting health for this unit.")]
        [SerializeField] protected float health = 100f;
        [Tooltip("The maximum health for this unit.")]
        [SerializeField] protected float maxHealth = 100f;
        [Tooltip("A speed multiplier for this specific unit.")]
        [SerializeField] protected float speedFactor = 1f;
        #endregion
        #region Inspector Validation
        protected virtual void OnValidate()
        {
            // Keep inspector fields valid.
            if (health < float.Epsilon)
                health = float.Epsilon;
            if (maxHealth < float.Epsilon)
                maxHealth = float.Epsilon;
            if (speedFactor < float.Epsilon)
                speedFactor = float.Epsilon;
        }
        #endregion
        #region IEditorWrapper Implementation
        /// <summary>
        /// Retrieves the combat actor specified by the scene instance.
        /// </summary>
        /// <returns>The combat actor.</returns>
        public override abstract CombatActor Unwrap();
        #endregion
    }
}
