namespace AI_Midterm_RTS.UnityCore
{
    /// <summary>
    /// Implements instantiation for an object type.
    /// </summary>
    /// <typeparam name="T">The object to instantiate.</typeparam>
    public interface IInstantiator<T>
    {
        #region Method Requirements
        /// <summary>
        /// Creates a copy of the requested class.
        /// </summary>
        /// <returns>The target element of the copy.</returns>
        T Instantiate();
        #endregion
    }
}
