using UnityEngine;

namespace AI_Midterm_RTS.EngineInterop.Unity
{
    /// <summary>
    /// Implements a fixed update tick provider in Unity.
    /// </summary>
    public sealed class UnityFixedUpdateTickProvider : MonoBehaviour, ITickProvider
    {
        #region ITickProvider Event
        /// <summary>
        /// Called every Update tick.
        /// </summary>
        public event TickListener Tick;
        #endregion
        #region Monobehaviour Implementation
        private void FixedUpdate()
        {
            Tick?.Invoke(Time.fixedDeltaTime);
        }
        private void OnDestroy()
        {
            Tick = null;
        }
        #endregion
    }
}
