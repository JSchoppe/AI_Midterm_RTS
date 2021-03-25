using UnityEngine;

using AI_Midterm_RTS.UnityCore.Extensions;

namespace AI_Midterm_RTS.Indicators.Unity
{
    // TODO this should be wrapped by an object pool.
    /// <summary>
    /// Implements an arrow using 2D or 3D scalable assets in Unity.
    /// </summary>
    public sealed class UnityArrow : MonoBehaviour, IArrow
    {
        #region Fields
        private bool isHidden;
        private Vector3 start, end;
        #endregion
        #region Inspector Fields
        [Header("Arrow References")]
        [Tooltip("The transform that will be scaled along the z-axis to match the arrow length.")]
        [SerializeField] private Transform baseTransform = default;
        [Tooltip("The transform that will be placed at the arrow tip with forward pointing in the arrow direction.")]
        [SerializeField] private Transform endTransform = default;
        [Tooltip("All renderers that need to be hidden/unhidden for this arrow.")]
        [SerializeField] private Renderer[] allRenderers = default;
        [Header("Arrow Attributes")]
        [Tooltip("The length of the header.")]
        [SerializeField] private float headerSize = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Prevent invalid inspector fields.
            headerSize = Mathf.Max(0f, headerSize);
        }
        #endregion
        #region Initialization
        private void Awake()
        {
            // Hide the arrow by default.
            IsHidden = true;
        }
        #endregion
        #region IArrow Properties
        /// <summary>
        /// Whether this arrow is currently displayed.
        /// </summary>
        public bool IsHidden
        {
            get => isHidden;
            set
            {
                if (value != isHidden)
                {
                    isHidden = value;
                    // Hide or show the renderers in the scene.
                    foreach (Renderer renderer in allRenderers)
                        renderer.enabled = !isHidden;
                }
            }
        }
        /// <summary>
        /// The base of the arrow.
        /// </summary>
        public Vector3 Base
        {
            get => start;
            set
            {
                if (value != start)
                {
                    start = value;
                    ReorientArrow();
                }
            }
        }
        /// <summary>
        /// The end of the arrow.
        /// </summary>
        public Vector3 End
        {
            get => end;
            set
            {
                if (value != end)
                {
                    end = value;
                    ReorientArrow();
                }
            }
        }
        #endregion
        #region Calculate Arrow
        private void ReorientArrow()
        {
            // Set the end at the end of the line.
            endTransform.position = end;
            // Get the direction of the arrow safely.
            Vector3 forwardsVector = end - start;
            if (forwardsVector == Vector3.zero)
                forwardsVector = Vector3.forward;
            else
                forwardsVector = Vector3.Normalize(forwardsVector);
            // Scale the arrow base to reach the end.
            baseTransform.SetLocalScaleZ(
                Mathf.Max(
                    0f,
                    Vector3.Distance(start, end) - headerSize));
            // Orient the direction of both transforms.
            baseTransform.LookAt(baseTransform.position + forwardsVector);
            endTransform.LookAt(endTransform.position + forwardsVector);
        }
        #endregion
    }
}
