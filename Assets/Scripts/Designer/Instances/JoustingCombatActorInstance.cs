using UnityEngine;

using AI_Midterm_RTS.AIActors;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of a jousting combat actor.
    /// </summary>
    public sealed class JoustingCombatActorInstance : CombatActorInstance
    {
        #region Instance Field
        private JoustingCombatActor instance;
        #endregion
        #region Inspector Fields
        [Header("Jousting Actor Attributes")]
        [Tooltip("The range at which a joust can be started.")]
        [SerializeField] private float joustingRange = 1f;
        [Tooltip("The speed of the jousting movement.")]
        [SerializeField] private float joustingSpeed = 1f;
        [Tooltip("The damage dealt to enemies while hit by jousting.")]
        [SerializeField] private float joustingDamage = 1f;
        #endregion
        #region Inspector Validation
        protected sealed override void OnValidate()
        {
            base.OnValidate();
            // Make sure inspector values are valid.
            if (joustingRange < 0f)
                joustingRange = 0f;
            if (joustingSpeed < float.Epsilon)
                joustingSpeed = float.Epsilon;
            if (joustingDamage < 0f)
                joustingDamage = 0f;
        }
        #endregion
        #region IEditorWrapper Implementation
        public override CombatActor Unwrap()
        {
            if (instance == null)
            {
                // Create the actor and set the values
                // based on the inspector fields.
                instance = JoustingCombatActor.MakeActor(transform);
                instance.HealthMeter = healthMeter;
                instance.Navigator = navigator;
                instance.StateTable = table.Unwrap();
                instance.Health = health;
                instance.MaxHealth = maxHealth;
                instance.SpeedFactor = speedFactor;
                instance.JoustingRange = joustingRange;
                instance.JoustingSpeed = joustingSpeed;
                instance.JoustingDamage = joustingDamage;
                // Initialize the state of this actor.
                instance.EvaluateStateChange();
            }
            return instance;
        }
        #endregion
    }
}
