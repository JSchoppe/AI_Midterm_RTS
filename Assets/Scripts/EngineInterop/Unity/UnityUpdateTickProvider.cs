using UnityEngine;

namespace AI_Midterm_RTS.EngineInterop.Unity
{
    /// <summary>
    /// Implements an update tick provider in Unity.
    /// </summary>
    public sealed class UnityUpdateTickProvider : MonoBehaviour, ITickProvider
    {
        #region ITickProvider Event
        /// <summary>
        /// Called every Update tick.
        /// </summary>
        public event TickListener Tick;
        #endregion
        #region Monobehaviour Implementation
        private void Update()
        {
            Tick?.Invoke(Time.deltaTime);
        }
        private void OnDestroy()
        {
            Tick = null;
        }
        #endregion
    }
}
