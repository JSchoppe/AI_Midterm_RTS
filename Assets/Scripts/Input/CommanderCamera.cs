using UnityEngine;
using UnityEngine.InputSystem;

namespace AI_Midterm_RTS.Input
{
    /// <summary>
    /// Implements the camera movement for the commander.
    /// </summary>
    public sealed class CommanderCamera : MonoBehaviour
    {
        #region State Fields
        private Vector2 cameraMoveInput;
        #endregion
        #region Inspector Fields
        [Tooltip("The movement speed of the camera.")]
        [SerializeField] private float speed = 5f;
        #endregion
        #region MonoBehaviour Implementation
        private void Update()
        {
            // Move the camera in the direction of the input.
            Vector3 deltaMovement =
                new Vector3(cameraMoveInput.x, 0f, cameraMoveInput.y).normalized;
            transform.position += speed * Time.deltaTime * deltaMovement;
        }
        #endregion
        #region Input System Listeners
        /// <summary>
        /// Recieves input from the new input system for camera movement.
        /// </summary>
        /// <param name="context">The input system context.</param>
        public void RecieveStickInput(InputAction.CallbackContext context)
        {
            cameraMoveInput = context.ReadValue<Vector2>();
        }
        #endregion
    }
}
