namespace AI_Midterm_RTS.Indicators
{
    /// <summary>
    /// Implements a scene behaviour the draws a meter over an actor.
    /// </summary>
    public interface IMeter : IToggleable
    {
        #region Property Requirements
        /// <summary>
        /// When true the meter will hide when filled completely.
        /// </summary>
        bool AutoHidesAtFill { get; set; }
        #endregion
        #region Method Requirements
        /// <summary>
        /// Sets the fill level on the meter.
        /// </summary>
        /// <param name="fillValue">The current fill level.</param>
        /// <param name="maxValue">The max fill amount.</param>
        void SetMeter(float fillValue, float maxValue);
        #endregion
    }
}
