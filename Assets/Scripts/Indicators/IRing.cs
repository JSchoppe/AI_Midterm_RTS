using UnityEngine; // TODO wrap Vector3, script should be engine agnostic.

namespace AI_Midterm_RTS.Indicators
{
    /// <summary>
    /// Implements the drawing of a ring.
    /// </summary>
    public interface IRing
    {
        #region Properties Required
        /// <summary>
        /// Whether this ring is currently rendered.
        /// </summary>
        bool IsHidden { get; set; }
        /// <summary>
        /// The location of the ring.
        /// </summary>
        Vector3 Location { get; set; }
        /// <summary>
        /// The radius of the ring.
        /// </summary>
        float Radius { get; set; }
        #endregion
    }
}
