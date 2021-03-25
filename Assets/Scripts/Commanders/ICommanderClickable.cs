using UnityEngine; // TODO wrap Vector3, script should be engine agnostic.

using AI_Midterm_RTS.Input;

namespace AI_Midterm_RTS.Commanders
{
    // TODO shouldn't be clickable, should be generic to any button down event.
    /// <summary>
    /// Implements behaviour for when a commander clicks on this object.
    /// </summary>
    public interface ICommanderClickable
    {
        #region Method Requirements
        /// <summary>
        /// Called when a commander begins a drag of this object.
        /// </summary>
        /// <param name="cursor">The cursor that clicked.</param>
        /// <param name="commander">The commander that pressed click.</param>
        /// <param name="startPosition">The starting position in 3D space.</param>
        void ClickEnter(CommanderCursor cursor, Commander commander, Vector3 startPosition);
        /// <summary>
        /// Called when a commander ends a drag of this object.
        /// </summary>
        /// <param name="cursor">The cursor that releases.</param>
        /// <param name="commander">The commander that released click.</param>
        /// <param name="endPosition">The ending position in 3D space.</param>
        void ClickExit(CommanderCursor cursor, Commander commander, Vector3 endPosition);
        #endregion
    }
}
