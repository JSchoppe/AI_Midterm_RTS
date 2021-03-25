using System;
using UnityEngine; // TODO Wrap Vector3. This script should be engine agnostic.

namespace AI_Midterm_RTS.Navigation
{
    /// <summary>
    /// Implements an agent that navigates through a 3D scene.
    /// </summary>
    public interface INavigator
    {
        #region Broadcasters Required
        /// <summary>
        /// Called when the target destination is reached.
        /// </summary>
        Action OnDestinationReached { set; }
        /// <summary>
        /// Called if the navigation fails.
        /// </summary>
        Action OnNavigationFailed { set; }
        #endregion
        #region Propertys Required
        /// <summary>
        /// The current navigation target.
        /// </summary>
        Vector3 Target { get; set; }
        /// <summary>
        /// The current navigation speed.
        /// </summary>
        float Speed { get; set; }
        /// <summary>
        /// How far from the destination is considered at the destination.
        /// </summary>
        float DestinationTolerance { get; set; }
        /// <summary>
        /// Whether the navigator is currently ticking.
        /// </summary>
        bool IsPaused { get; set; }
        #endregion
        #region Methods Required
        /// <summary>
        /// Clears the current route on the agent.
        /// </summary>
        void QuitRoute();
        /// <summary>
        /// Teleports the navigator to the requested position,
        /// ignoring normal navigation procedures.
        /// </summary>
        /// <param name="target">The target location.</param>
        void Teleport(Vector3 target);
        #endregion
    }
}
