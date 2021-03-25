using UnityEngine;
using UnityEngine.InputSystem;

using AI_Midterm_RTS.Commanders;
using AI_Midterm_RTS.Designer.Instances;

namespace AI_Midterm_RTS.Input
{
    // TODO some of this functionality could likely
    // be abstracted away from the engine.
    /// <summary>
    /// Implements the behaviour that allows the commander
    /// cursor to interact with cursor objects.
    /// </summary>
    public sealed class CommanderCursor : MonoBehaviour
    {
        #region State Fields
        private bool inClick;
        private ICommanderFocusable currentFocused;
        private ICommanderClickable currentClickable;
        #endregion
        #region Inspector Fields
        [Header("Cursor References")]
        [Tooltip("The camera used by the commander.")]
        [SerializeField] private Camera commanderCamera = default;
        [Tooltip("The initial commander linked to this cursor.")]
        [SerializeField] private PlayerCommanderInstance commander = default;
        [Header("Cursor Attributes")]
        [Tooltip("Filters out objects that should not be cast against for interaction.")]
        [SerializeField] private LayerMask layerMask = default;
        [Tooltip("The distance in meters to check the cursor raycast.")]
        [SerializeField] private float raycastDistance = 20f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Prevent invalid inspector data.
            raycastDistance = Mathf.Max(0f, raycastDistance);
        }
        #endregion
        #region Properties
        /// <summary>
        /// The commander context for this cursor.
        /// </summary>
        public Commander LinkedCommander { get; set; }
        /// <summary>
        /// The current location in space that the cursor is hitting.
        /// </summary>
        public Vector3 CurrentHitPosition { get; set; }
        #endregion
        #region MonoBehaviour Implementation
        private void Awake()
        {
            // Get the instance of the commander
            // if specified by the designer.
            if (commander != null)
                LinkedCommander = commander.Unwrap();
        }
        private void Update()
        {
            // Only do raycasting if the linked commander
            // has been set. Also skip raycasting if the
            // click is active.
            if (LinkedCommander != null)
            {
                // Cast a ray against the scene.
                if (Physics.Raycast(
                    commanderCamera.ScreenPointToRay(Mouse.current.position.ReadValue()),
                    out RaycastHit hit,
                    raycastDistance,
                    layerMask))
                {
                    // Set the latest hit position.
                    CurrentHitPosition = hit.point;
                    // If not in the clicking state, check
                    // for updates in the hovered objects.
                    if (!inClick)
                    {
                        // Check to see if there is a focusable component.
                        ICommanderFocusable focusable =
                            hit.collider.GetComponent<ICommanderFocusable>();
                        if (focusable != null)
                        {
                            // Is this a new focused item?
                            if (focusable != currentFocused)
                            {
                                // Update the focused states.
                                if (currentFocused != null)
                                    currentFocused.FocusedExit(this, LinkedCommander);
                                currentFocused = focusable;
                                currentFocused.FocusedEnter(this, LinkedCommander);
                            }
                        }
                        else if (currentFocused != null)
                        {
                            // If we are not hovering on a focusable item,
                            // then clear any focused state.
                            currentFocused.FocusedExit(this, LinkedCommander);
                            currentFocused = null;
                        }
                        // Check to see if there is a clickable component.
                        currentClickable =
                            hit.collider.GetComponent<ICommanderClickable>();
                    }
                }
            }
        }
        #endregion
        #region Click Listeners
        private void OnClicked()
        {
            inClick = true;
            // Clear any focused state.
            if (currentFocused != null)
            {
                currentFocused.FocusedExit(this, LinkedCommander);
                currentFocused = null;
            }
            // Trigger clicked enter state.
            if (currentClickable != null)
                currentClickable.ClickEnter(this, LinkedCommander, CurrentHitPosition);
        }
        private void OnReleased()
        {
            inClick = false;
            // Exit the clicked state.
            if (currentClickable != null)
                currentClickable.ClickExit(this, LinkedCommander, CurrentHitPosition);
        }
        #endregion
        #region Input System Listeners
        /// <summary>
        /// Used by the Unity Input System to invoke on clicked behaviour.
        /// </summary>
        /// <param name="context">The input action context.</param>
        public void RecieveClicked(InputAction.CallbackContext context)
        {
            // Is the mouse button down or up?
            if (context.ReadValueAsButton() && !context.performed)
                OnClicked();
            else if (!context.ReadValueAsButton())
                OnReleased();
        }
        #endregion
    }
}
