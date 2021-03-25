using UnityEngine;

using AI_Midterm_RTS.EngineInterop.Unity;
using AI_Midterm_RTS.Bases;
using AI_Midterm_RTS.Commanders;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance for a player controlled commander.
    /// </summary>
    public sealed class PlayerCommanderInstance : UnityEditorWrapper<PlayerCommander>
    {
        #region Instance Field
        private PlayerCommander commander;
        #endregion
        #region Inspector Fields
        [Tooltip("The team that this commander is associated with.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("The maximum number of units that can be deployed.")]
        [SerializeField] private int maxUnitCount = 50;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Ensure valid inspector values.
            maxUnitCount = Mathf.Max(0, maxUnitCount);
        }
        #endregion
        #region IEditorWrapper Implementation
        public override PlayerCommander Unwrap()
        {
            // Initialize the commander instance.
            if (commander == null)
            {
                commander = new PlayerCommander(teamID)
                {
                    // Initialize Fields.
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
