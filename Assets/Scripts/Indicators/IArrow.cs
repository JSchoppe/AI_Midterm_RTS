using UnityEngine; // TODO wrap Vector3, script should be engine agnostic.

namespace AI_Midterm_RTS.Indicators
{
    /// <summary>
    /// Implements the drawing of a linear arrow.
    /// </summary>
    public interface IArrow
    {
        #region Properties Required
        /// <summary>
        /// Whether this arrow is currently rendered.
        /// </summary>
        bool IsHidden { get; set; }
        /// <summary>
        /// The start of the arrow.
        /// </summary>
        Vector3 Base { get; set; }
        /// <summary>
        /// The end of the arrow.
        /// </summary>
        Vector3 End { get; set; }
        #endregion
    }
}
