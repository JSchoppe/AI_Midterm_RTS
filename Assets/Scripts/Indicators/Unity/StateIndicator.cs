using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AI_Midterm_RTS.AICore;
using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.Designer.Instances;

namespace AI_Midterm_RTS.Indicators.Unity
{
    // TODO this class is wasteful creating identical dictionaries
    // for every actor.
    /// <summary>
    /// An indicator that renders the actor state to the screen.
    /// </summary>
    public sealed class StateIndicator : MonoBehaviour
    {
        #region Fields
        private CombatActor instance;
        private Dictionary<CombatActor.State, Material> materials;
        #endregion
        #region Inspector Fields
        [Header("Indicator References")]
        [Tooltip("The combat actor instance that will run this indicator.")]
        [SerializeField] private CombatActorInstance combatActor = default;
        [Tooltip("The mesh renderer whose material reflects state change.")]
        [SerializeField] private MeshRenderer indicatorRenderer = default;
        [Tooltip("The material bindings for each state.")]
        [SerializeField] private StateMaterialEntry[] stateMaterials = default;
        [Serializable]
        private sealed class StateMaterialEntry
        {
            [HideInInspector] public string name;
            [Tooltip("The state of the combat actor.")]
            public CombatActor.State state;
            [Tooltip("The material to represent the state.")]
            public Material material;
        }
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Label the table entries with the enum name.
            if (stateMaterials != null)
                foreach (StateMaterialEntry entry in stateMaterials)
                    entry.name =
                        ObjectNames.NicifyVariableName(entry.state.ToString());
        }
        #endregion
        #region Initialization + Destruction
        private void Awake()
        {
            // Convert the inspector materials into dictionary form.
            materials = new Dictionary<CombatActor.State, Material>();
            foreach (StateMaterialEntry entry in stateMaterials)
                materials.Add(entry.state, entry.material);
            // Get the combat actor instance.
            instance = combatActor.Unwrap();
            // Subscribe to the state changed event and set initial visual state.
            instance.StateChanged += OnStateChanged;
            OnStateChanged(instance, instance.CurrentState);
        }
        private void OnDestroy()
        {
            // Unbind from events.
            instance.StateChanged -= OnStateChanged;
        }
        #endregion
        #region State Changed Listener
        private void OnStateChanged(StateMachine<CombatActor.State> machine, CombatActor.State state)
        {
            indicatorRenderer.material = materials[state];
        }
        #endregion
    }
}
