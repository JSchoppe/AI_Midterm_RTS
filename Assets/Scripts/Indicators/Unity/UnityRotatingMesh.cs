using UnityEngine;

namespace AI_Midterm_RTS.Indicators.Unity
{
    // TODO could be further abstracted away from the engine.
    // TODO this class could be generalized to include rotation
    // about any axis.
    /// <summary>
    /// Implements a rotating mesh that can be toggled in Unity.
    /// </summary>
    public sealed class UnityRotatingMesh : MonoBehaviour, IToggleable
    {
        #region Fields
        private bool isToggled;
        #endregion
        #region Inspector Fields
        [Tooltip("The rotation speed of the mesh in degrees per second.")]
        [SerializeField] private float rotationDegreesPerSecond = 90f;
        [Tooltip("The renderers associated with this script.")]
        [SerializeField] private Renderer[] renderersToShow = default;
        #endregion
        #region IToggleable Properties
        /// <summary>
        /// Whether the rotating mesh is currently visible.
        /// </summary>
        public bool IsToggled
        {
            get => isToggled;
            set
            {
                isToggled = value;
                // Update the visual state of the renderers.
                foreach (Renderer renderer in renderersToShow)
                    renderer.enabled = isToggled;
            }
        }
        #endregion
        #region IToggleable Methods
        /// <summary>
        /// Flips whether the meshes are currently being rendered.
        /// </summary>
        public void Toggle() => IsToggled = !IsToggled;
        #endregion
        #region MonoBehaviour Implementation
        private void Update()
        {
            // Update the revolution of the mesh.
            transform.eulerAngles += Vector3.up
                * Time.deltaTime * rotationDegreesPerSecond;
        }
        #endregion
    }
}
