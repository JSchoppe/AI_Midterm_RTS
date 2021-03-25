namespace AI_Midterm_RTS.EngineInterop
{
    #region Event Delegates
    /// <summary>
    /// Function that responds to Ticking routines.
    /// </summary>
    /// <param name="deltaTime">The change in time in seconds.</param>
    public delegate void TickListener(float deltaTime);
    #endregion
    /// <summary>
    /// Implements tick events that pass time deltas.
    /// </summary>
    public interface ITickProvider
    {
        #region Event Requirements
        /// <summary>
        /// Called on tick as specified by the provider.
        /// Passes change in time since last tick.
        /// </summary>
        event TickListener Tick;
        #endregion
    }
}
