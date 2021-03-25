using UnityEngine;

using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Designer.Instances;

namespace AI_Midterm_RTS.UnityCore
{
    /// <summary>
    /// Wraps the instantiation procedure in Unity.
    /// </summary>
    public sealed class PrefabInstantiator : MonoBehaviour, IInstantiator<CombatActor>
    {
        #region Inspector Fields
        [Tooltip("The prefab to instantiate.")]
        [SerializeField] private GameObject prefab = default;
        #endregion
        #region IInstantiator Implementation
        /// <summary>
        /// Instantiates and returns the template prefab.
        /// </summary>
        /// <returns>A copy of the prefab.</returns>
        public CombatActor Instantiate()
        {
            return Instantiate(prefab).GetComponent<CombatActorInstance>().Unwrap();
        }
        #endregion
    }
}
