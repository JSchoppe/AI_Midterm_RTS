using System;
using UnityEngine;
using UnityEditor;

using AI_Midterm_RTS.EngineInterop.Unity;
using AI_Midterm_RTS.AICore.Distributions;
using AI_Midterm_RTS.AIActors;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of a weighted table of combat actor states.
    /// </summary>
    public sealed class CombatActorStateWeightedTableInstance
        : UnityEditorWrapper<WeightedTable<CombatActor.State>>
    {
        #region Inspector Fields
        [Header("Weighted Table")]
        [Tooltip("The table entries for combat actor states.")]
        [SerializeField] private TableEntry[] entries = default;
        [Serializable]
        private sealed class TableEntry
        {
            [HideInInspector] public string name;
            [Tooltip("The weight linked to this result.")]
            public float weight;
            [Tooltip("The state linked to this result.")]
            public CombatActor.State result;
        }
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Ensure all weights are non-negative.
            if (entries != null)
            {
                foreach (TableEntry entry in entries)
                {
                    if (entry.weight < 0f)
                        entry.weight = 0f;
                    // Label the entry by the state.
                    entry.name = ObjectNames.NicifyVariableName(entry.result.ToString());
                }
            }
        }
        #endregion
        #region IEditorWrapper Implementation
        /// <summary>
        /// Retrieves the weighted table specified by the scene instance.
        /// </summary>
        /// <returns>A copy of the weighted table for combat actor state.</returns>
        public override WeightedTable<CombatActor.State> Unwrap()
        {
            // Construct a new table from the given inspector fields.
            var table = new WeightedTable<CombatActor.State>();
            if (entries != null)
                foreach (TableEntry entry in entries)
                    table.AddResult(entry.weight, entry.result);
            return table;
        }
        #endregion
    }
}
