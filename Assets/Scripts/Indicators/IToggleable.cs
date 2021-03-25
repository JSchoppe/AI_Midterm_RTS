namespace AI_Midterm_RTS.Indicators
{
    /// <summary>
    /// Implements a feature that can be toggled on and off.
    /// </summary>
    public interface IToggleable
    {
        #region Property Requirements
        /// <summary>
        /// Sets whether the feature is toggled.
        /// </summary>
        bool IsToggled { get; set; }
        #endregion
        #region Method Requirements
        /// <summary>
        /// Flips the current IsToggled state.
        /// </summary>
        void Toggle();
        #endregion
    }
}
