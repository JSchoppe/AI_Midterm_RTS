namespace AI_Midterm_RTS.Commanders
{
    /// <summary>
    /// Implements a listener for when this object is near a commander.
    /// </summary>
    public interface INearCommanderListener
    {
        #region Method Requirements
        /// <summary>
        /// Called when this object has become near to a commander.
        /// </summary>
        void NearbyEnter();
        /// <summary>
        /// Called when this object is no longer near a commander.
        /// </summary>
        void NearbyExit();
        #endregion
    }
}
