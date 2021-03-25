namespace AI_Midterm_RTS.AICore.Distributions
{
    /// <summary>
    /// Implements a distribution for floating point values.
    /// </summary>
    public abstract class FloatDistribution : IDistribution<float>
    {
        #region IDistribution Implementation
        /// <summary>
        /// Calls the distribution to retrieve or generate the next number.
        /// </summary>
        /// <returns>The next float from the distribution.</returns>
        public abstract float Next();
        #endregion
    }
}
