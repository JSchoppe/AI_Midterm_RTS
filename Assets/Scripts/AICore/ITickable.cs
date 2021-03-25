namespace AI_Midterm_RTS.AICore
{
    /// <summary>
    /// Defines an object that can be ticked.
    /// </summary>
    public interface ITickable
    {
        #region Method Requirements
        /// <summary>
        /// Method that id called in intervals.
        /// </summary>
        /// <param name="deltaTime">The elapsed time in seconds since the last tick.</param>
        void Tick(float deltaTime);
        #endregion
    }
}
