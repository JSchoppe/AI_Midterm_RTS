namespace AI_Midterm_RTS.AICore
{
    /// <summary>
    /// Defines the implementation required to work with the
    /// state machine model.
    /// </summary>
    public interface IState
    {
        #region Method Requirements
        /// <summary>
        /// Called when a state is requested to be entered.
        /// </summary>
        void StateEntered();
        /// <summary>
        /// Called when another state is entered. This
        /// may interrupt the current behaviour state.
        /// </summary>
        void StateExited();
        #endregion
    }
}
