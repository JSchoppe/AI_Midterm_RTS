using UnityEngine;

namespace AI_Midterm_RTS.Indicators.Unity
{
    // TODO this should be wrapped by an object pool.
    // TODO would be useful to add ring normal property.
    /// <summary>
    /// Implements a ring using 2D or 3D scalable assets in Unity.
    /// </summary>
    public sealed class UnityRing : MonoBehaviour, IRing
    {
        #region Fields
        private bool isHidden;
        private Vector3 location;
        private float radius;
        #endregion
        #region Inspector Fields
        [Header("Ring References")]
        [Tooltip("The ring transfrom that will be scaled along the x and z axes according to radius.")]
        [SerializeField] private Transform ringTransform = default;
        [Tooltip("All renderers that need to be hidden/unhidden for this ring.")]
        [SerializeField] private Renderer[] allRenderers = default;
        #endregion
        #region Initialization
        private void Awake()
        {
            // Hide the ring by default.
            IsHidden = true;
        }
        #endregion
        #region IRing Properties
        /// <summary>
        /// Whether this ring is currently displayed.
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
        /// The location of the center of the ring in 3D space.
        /// </summary>
        public Vector3 Location
        {
            get => location;
            set
            {
                if (value != location)
                {
                    location = value;
                    // Update the ring location.
                    ringTransform.position = location;
                }
            }
        }
        /// <summary>
        /// The radius of the ring in Unity meters.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                value = Mathf.Max(value, 0f);
                if (value != radius)
                {
                    radius = value;
                    // Update the ring transform scale.
                    // This is based in diameter.
                    ringTransform.localScale = new Vector3
                    {
                        x = radius * 2f,
                        y = ringTransform.localScale.y,
                        z = radius * 2f
                    };
                }
            }
        }
        #endregion
    }
}
