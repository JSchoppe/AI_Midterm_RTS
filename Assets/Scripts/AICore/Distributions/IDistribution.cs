namespace AI_Midterm_RTS.AICore.Distributions
{
    /// <summary>
    /// Implements a distribution pulling mechanism.
    /// </summary>
    /// <typeparam name="T">The value to pull in the distribution.</typeparam>
    public interface IDistribution<T>
    {
        #region Method Requirements
        /// <summary>
        /// Gets the next element from the distribution.
        /// </summary>
        /// <returns>An element or value in the distribution.</returns>
        T Next();
        #endregion
    }
}
