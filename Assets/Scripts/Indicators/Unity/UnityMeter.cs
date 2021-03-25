using UnityEngine;

using AI_Midterm_RTS.UnityCore.Extensions;

namespace AI_Midterm_RTS.Indicators.Unity
{
    /// <summary>
    /// An overhead meter, implemented in Unity.
    /// </summary>
    public sealed class UnityMeter : MonoBehaviour, IMeter
    {
        #region State Fields
        private float targetScale;
        private bool isFull;
        private bool isToggled;
        private bool autoHidesAtFill;
        #endregion
        #region Inspector Fields
        [Header("Meter References")]
        [Tooltip("All renderers that need to be toggle for this meter.")]
        [SerializeField] private Renderer[] allRenderers = default;
        [Header("Meter Attributes")]
        [Tooltip("Scales with the instant fill level along the z-axis.")]
        [SerializeField] private Transform instantFill = default;
        [Tooltip("Scales with the delayed fill level along the z-axis.")]
        [SerializeField] private Transform delayedFill = default;
        [Tooltip("Defines the scale z value at no meter fill.")]
        [SerializeField] private float minScaleZ = 0f;
        [Tooltip("Define the scale z value at full meter fill.")]
        [SerializeField] private float maxScaleZ = 1f;
        [Tooltip("The speed at which the meter animates.")]
        [SerializeField] private float animationSpeed = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Enforce a range greater than zero.
            if (minScaleZ < 0f)
                minScaleZ = 0f;
            if (maxScaleZ < 0f)
                maxScaleZ = 0f;
            if (maxScaleZ < minScaleZ)
                maxScaleZ = minScaleZ;
            // Enfore non-negative animation speed.
            if (animationSpeed < float.Epsilon)
                animationSpeed = float.Epsilon;
        }
        #endregion
        #region MonoBehaviour Implementation
        private void Awake()
        {
            // Set the meter to filled by default.
            SetMeter(1f, 1f);
        }
        private void Update()
        {
            // TODO this should not have to run every single frame.
            // Should bind/unbind based on when it has to update.
            if (delayedFill.transform.localScale.z != targetScale)
            {
                // Calculate the travel and difference.
                float deltaZ = Time.deltaTime * animationSpeed;
                float difference = targetScale - delayedFill.transform.localScale.z;
                // Will we step over the target?
                if (Mathf.Abs(difference) < deltaZ)
                    delayedFill.SetLocalScaleZ(targetScale);
                else
                {
                    // If not just move towards the target value.
                    if (difference > 0f)
                        delayedFill.SetLocalScaleZ(delayedFill.transform.localScale.z + deltaZ);
                    else
                        delayedFill.SetLocalScaleZ(delayedFill.transform.localScale.z - deltaZ);
                }
            }
        }
        #endregion
        #region IToggleable Properties
        /// <summary>
        /// Whether the meter is currently displayed.
        /// </summary>
        public bool IsToggled
        {
            get => isToggled;
            set
            {
                isToggled = value;
                // Update the renderers.
                foreach (Renderer renderer in allRenderers)
                    renderer.enabled = isToggled;
            }
        }
        #endregion
        #region IToggleable Methods
        /// <summary>
        /// Toggles whether the meter is displayed.
        /// </summary>
        public void Toggle() => IsToggled = !IsToggled;
        #endregion
        #region IMeter Properties
        /// <summary>
        /// Whether this meter automatically hides when full.
        /// </summary>
        public bool AutoHidesAtFill
        {
            get => autoHidesAtFill;
            set
            {
                if (value != autoHidesAtFill)
                {
                    // Check to update the visual state.
                    if (autoHidesAtFill == false)
                        IsToggled = true;
                    else
                        IsToggled = isFull;
                }
            }
        }
        #endregion
        #region IMeter Methods
        /// <summary>
        /// Sets the fill on the meter by scaling the gameobject.
        /// </summary>
        /// <param name="fillValue">The current fill value.</param>
        /// <param name="maxValue">The current max value.</param>
        public void SetMeter(float fillValue, float maxValue)
        {
            // Store whether this is full or not.
            isFull = (fillValue >= maxValue);
            // Do we need to update the visible state?
            if (AutoHidesAtFill)
                IsToggled = isFull;
            // Calculate how much we need to scale this by.
            targetScale = Mathf.Lerp(minScaleZ, maxScaleZ, fillValue / maxValue);
            // Instantly update the instant bar.
            instantFill.SetLocalScaleZ(targetScale);
        }
        #endregion
    }
}
