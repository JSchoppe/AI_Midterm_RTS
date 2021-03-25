namespace AI_Midterm_RTS.EngineInterop
{
    /// <summary>
    /// Implemented by classes that wrap simpler classes
    /// to be shown in an editor.
    /// </summary>
    /// <typeparam name="T">The simple class or struct being wrapped.</typeparam>
    public interface IEditorWrapper<T>
    {
        #region Method Requirements
        /// <summary>
        /// Gets the underlying class without editor context.
        /// </summary>
        /// <returns>The wrapped object.</returns>
        T Unwrap();
        #endregion
    }
}
