using UnityEngine;

using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.AIActors.MeleeCombatActor;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of a melee combat actor.
    /// </summary>
    public sealed class MeleeCombatActorInstance : CombatActorInstance
    {
        #region Instance Field
        private MeleeCombatActor instance;
        #endregion
        #region Inspector Fields
        [Header("Melee Actor Attributes")]
        [Tooltip("The range of melee attacks in meters.")]
        [SerializeField] private float range = 1f;
        [Tooltip("The delay between melee attacks.")]
        [SerializeField] private float attackDelay = 1f;
        [Tooltip("The damage dealt per melee attack.")]
        [SerializeField] private float attackDamage = 1f;
        #endregion
        #region Inspector Validation
        protected sealed override void OnValidate()
        {
            base.OnValidate();
            // Make sure inspector values are valid.
            if (range < 0f)
                range = 0f;
            if (attackDelay < float.Epsilon)
                attackDelay = float.Epsilon;
            if (attackDamage < 0f)
                attackDamage = 0f;
        }
        #endregion
        #region IEditorWrapper Implementation
        public override CombatActor Unwrap()
        {
            if (instance == null)
            {
                // Create the actor and set the values
                // based on the inspector fields.
                instance = MeleeCombatActor.MakeActor();
                instance.HealthMeter = healthMeter;
                instance.Navigator = navigator;
                instance.Health = health;
                instance.MaxHealth = maxHealth;
                instance.SpeedFactor = speedFactor;
                instance.Range = range;
                instance.AttackDelay = attackDelay;
                instance.AttackDamage = attackDamage;
            }
            return instance;
        }
        #endregion
    }
}
