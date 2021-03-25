using UnityEngine;

namespace AI_Midterm_RTS.EngineInterop.Unity
{
    /// <summary>
    /// Base class for behaviours that wrap simpler classes.
    /// </summary>
    /// <typeparam name="T">The object being wrapped by a scene instance.</typeparam>
    public abstract class UnityEditorWrapper<T> : MonoBehaviour, IEditorWrapper<T>
    {
        #region MonoBehaviour Stripping
        /// <summary>
        /// Called to retrieve the underlying object.
        /// </summary>
        /// <returns>The simpler class that is wrapped.</returns>
        public abstract T Unwrap();
        #endregion
    }
}
