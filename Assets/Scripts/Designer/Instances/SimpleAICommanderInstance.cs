using UnityEngine;

using AI_Midterm_RTS.EngineInterop.Unity;
using AI_Midterm_RTS.Commanders;
using AI_Midterm_RTS.EngineInterop;
using AI_Midterm_RTS.Bases;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance for a simple AI commander.
    /// </summary>
    public sealed class SimpleAICommanderInstance : UnityEditorWrapper<SimpleAICommander>
    {
        #region Instance Field
        private SimpleAICommander commander;
        #endregion
        #region Inspector Fields
        [Tooltip("The team that this commander is associated with.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("The maximum number of units that can be deployed.")]
        [SerializeField] private int maxUnitCount = 50;
        [Tooltip("The time between action intervals for the AI.")]
        [SerializeField] private float thoughtInterval = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Ensures valid inspector fields.
            thoughtInterval = Mathf.Max(0f, thoughtInterval);
        }
        #endregion
        #region IEditorWrapper Implementation
        public override SimpleAICommander Unwrap()
        {
            // Create the instance if it doesn't exist.
            if (commander == null)
            {
                commander = new SimpleAICommander(teamID,
                    UnityTickService.GetProvider(UnityLoopType.Update))
                {
                    ThoughtInterval = thoughtInterval,
                    MaxUnitCount = maxUnitCount
                };
                // TODO abstract this to a base class!
                // Grab all existing factories and
                // register them to this commander.
                foreach (FactoryInstance factoryInstance in FindObjectsOfType(typeof(FactoryInstance)))
                {
                    // Unwrapping here initializes the factory.
                    Base factoryBase = factoryInstance.Unwrap();
                    // If this base belongs to this commander,
                    // then register it.
                    if (factoryBase.TeamID == teamID)
                        commander.RegisterBase(factoryBase);
                }
            }
            return commander;
        }
        #endregion
    }
}
