using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI_Midterm_RTS.Navigation.Unity
{
    /// <summary>
    /// A navigator implemented using the unity nav mesh.
    /// </summary>
    public sealed class UnityNavigator : MonoBehaviour, INavigator
    {
        #region Fields
        private Vector3 target;
        private bool isPaused;
        private bool hasTarget;
        #endregion
        #region Inspector Fields
        [Header("Navigation References")]
        [Tooltip("The nav mesh agent instance.")]
        [SerializeField] private NavMeshAgent agent = default;
        [Header("Navigation Attributes")]
        [Tooltip("The nav mesh agent speed.")]
        [SerializeField] private float speed = 1f;
        [Tooltip("The tolerance from the destination to consider arrived.")]
        [SerializeField] private float destinationTolerance = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Enforce valid inspector values.
            if (speed < float.Epsilon)
                speed = float.Epsilon;
            if (destinationTolerance < 0f)
                destinationTolerance = 0f;
        }
        #endregion
        #region INavigator Broadcasters
        /// <summary>
        /// Called when the nav agent reaches the destination.
        /// </summary>
        public Action OnDestinationReached { private get; set; }
        /// <summary>
        /// Called if the nav agent fails to generate a path.
        /// </summary>
        public Action OnNavigationFailed { private get; set; }
        #endregion
        #region INavigator Properties
        /// <summary>
        /// The current target location of the navmesh.
        /// </summary>
        public Vector3 Target
        {
            get => target;
            set
            {
                if (value != target)
                {
                    target = value;
                    hasTarget = true;
                    // Update the nav mesh with the new destination.
                    if (!isPaused)
                        agent.SetDestination(target);
                }
            }
        }
        /// <summary>
        /// The current speed of the agent.
        /// </summary>
        public float Speed
        {
            get => speed;
            set
            {
                // Don't let speed be negative.
                value = Mathf.Max(0f, value);
                if (value != speed)
                {
                    speed = value;
                    agent.speed = speed;
                }
            }
        }
        /// <summary>
        /// The tolerance to consider having reached the destination.
        /// </summary>
        public float DestinationTolerance
        {
            get => destinationTolerance;
            set
            {
                value = Mathf.Max(value, 0f);
                if (value != destinationTolerance)
                    destinationTolerance = value;
            }
        }
        /// <summary>
        /// Whether the navmesh should be traveling or not.
        /// </summary>
        public bool IsPaused
        {
            get => isPaused;
            set
            {
                if (value != isPaused)
                {
                    isPaused = value;
                    agent.isStopped = isPaused;
                    if (!isPaused && hasTarget)
                        agent.SetDestination(target);
                }
            }
        }
        #endregion
        #region INavigator Methods
        /// <summary>
        /// Quits the current nabmesh route.
        /// </summary>
        public void QuitRoute()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
        /// <summary>
        /// Teleports the nav mesh agent to a new position.
        /// </summary>
        /// <param name="target">The target position to teleport to.</param>
        public void Teleport(Vector3 target)
        {
            agent.transform.position = target;
        }
        #endregion
        #region MonoBehaviour Implementation
        private void Awake()
        {
            isPaused = false;
        }
        private void FixedUpdate()
        {
            // Are we currently traveling?
            if (hasTarget && !agent.pathPending)
            {
                // Did the navmesh fail to find a sufficient path?
                if (agent.pathStatus != NavMeshPathStatus.PathComplete)
                {
                    hasTarget = false;
                    OnDestinationReached?.Invoke();
                }
                // Did the navmesh reach the destination?
                if (agent.remainingDistance < DestinationTolerance)
                {
                    hasTarget = false;
                    OnDestinationReached?.Invoke();
                }
            }
        }
        #endregion
    }
}
