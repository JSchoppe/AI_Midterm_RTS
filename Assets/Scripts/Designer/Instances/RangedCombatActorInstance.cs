using UnityEngine;

using AI_Midterm_RTS.AIActors;
using AI_Midterm_RTS.AIActors.RangedCombatActor;

namespace AI_Midterm_RTS.Designer.Instances
{
    /// <summary>
    /// A scene instance of a ranged combat actor.
    /// </summary>
    public sealed class RangedCombatActorInstance : CombatActorInstance
    {
        #region Instance Field
        private RangedCombatActor instance;
        #endregion
        #region Inspector Fields
        [Header("Ranged Actor Attributes")]
        [Tooltip("The range of projectiles in meters.")]
        [SerializeField] private float range = 1f;
        [Tooltip("The delay between firing shots.")]
        [SerializeField] private float fireDelay = 1f;
        [Tooltip("The speed of the fired shots in meters per second.")]
        [SerializeField] private float shotSpeed = 1f;
        [Tooltip("The damage inflicted when a shot hits an enemy.")]
        [SerializeField] private float shotDamage = 1f;
        #endregion
        #region Inspector Validation
        protected sealed override void OnValidate()
        {
            base.OnValidate();
            // Make sure inspector values are valid.
            if (range < 0f)
                range = 0f;
            if (fireDelay < float.Epsilon)
                fireDelay = float.Epsilon;
            if (shotSpeed < float.Epsilon)
                shotSpeed = float.Epsilon;
            if (shotDamage < 0f)
                shotDamage = 0f;
        }
        #endregion
        #region IEditorWrapper Implementation
        public override CombatActor Unwrap()
        {
            if (instance == null)
            {
                // Create the actor and set the values
                // based on the inspector fields.
                instance = RangedCombatActor.MakeActor();
                instance.HealthMeter = healthMeter;
                instance.Navigator = navigator;
                instance.StateTable = table.Unwrap();
                instance.Health = health;
                instance.MaxHealth = maxHealth;
                instance.SpeedFactor = speedFactor;
                instance.Range = range;
                instance.FireDelay = fireDelay;
                instance.ShotSpeed = shotSpeed;
                instance.ShotDamage = shotDamage;
                // Initialize the state of this actor.
                instance.EvaluateStateChange();
            }
            return instance;
        }
        #endregion
    }
}
