using AI_Midterm_RTS.Input;

namespace AI_Midterm_RTS.Commanders
{
    /// <summary>
    /// Implements behaviour for when a commander focuses on this object.
    /// </summary>
    public interface ICommanderFocusable
    {
        #region Method Requirements
        /// <summary>
        /// Called when this object recieves focus from a commander.
        /// </summary>
        /// <param name="cursor">The cursor that hovered over this object.</param>
        /// <param name="commander">The focusing commander.</param>
        void FocusedEnter(CommanderCursor cursor, Commander commander);
        /// <summary>
        /// Called when this object loses focus from a commander.
        /// </summary>
        /// <param name="cursor">The cursor that hovered over this object.</param>
        /// <param name="commander">The previously focused commander.</param>
        void FocusedExit(CommanderCursor cursor, Commander commander);
        #endregion
    }
}
