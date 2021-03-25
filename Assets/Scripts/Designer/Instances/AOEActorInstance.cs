using UnityEngine;

using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.AIActors.AOECombatActor;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of an area of effect combat actor.
    /// </summary>
    public sealed class AOEActorInstance : CombatActorInstance
    {
        #region Instance Field
        private AOECombatActor instance;
        #endregion
        #region Inspector Fields
        [Header("AOE Actor Attributes")]
        [Tooltip("The delay between area of effect attacks.")]
        [SerializeField] private float delay = 1f;
        [Tooltip("The size of the area of effect radius.")]
        [SerializeField] private float radius = 1f;
        [Tooltip("The damage dealt to all enemies within the radius.")]
        [SerializeField] private float damage = 1f;
        #endregion
        #region Inspector Validation
        protected sealed override void OnValidate()
        {
            base.OnValidate();
            // Make sure inspector values are valid.
            if (delay < 0f)
                delay = 0f;
            if (radius < 0f)
                radius = 0f;
            if (damage < 0f)
                damage = 0f;
        }
        #endregion
        #region IEditorWrapper Implementation
        public override CombatActor Unwrap()
        {
            if (instance == null)
            {
                // Create the actor and set the values
                // based on the inspector fields.
                instance = AOECombatActor.MakeActor(transform);
                instance.HealthMeter = healthMeter;
                instance.Navigator = navigator;
                instance.StateTable = table.Unwrap();
                instance.Health = health;
                instance.MaxHealth = maxHealth;
                instance.SpeedFactor = speedFactor;
                instance.AttackDelay = delay;
                instance.AttackRadius = radius;
                instance.AttackDamage = damage;
                // Initialize the state of this actor.
                instance.EvaluateStateChange();
            }
            return instance;
        }
        #endregion
    }
}
