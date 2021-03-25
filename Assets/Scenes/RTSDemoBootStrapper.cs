using UnityEngine;
using UnityEngine.SceneManagement;

using AI_Midterm_RTS.Designer.Instances;
using AI_Midterm_RTS.Commanders;

namespace AI_Midterm_RTS.Scenes.BootStrappers
{
    /// <summary>
    /// Starts the demo scene.
    /// </summary>
    public sealed class RTSDemoBootStrapper : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The player commander.")]
        [SerializeField] private PlayerCommanderInstance player = default;
        [Tooltip("The opponent commander.")]
        [SerializeField] private SimpleAICommanderInstance opponent = default;
        #endregion
        #region Bootstrapping Procedure
        private void Start()
        {
            // Invoke the core classes to be constructed.
            Commander playerCommander = player.Unwrap();
            Commander opponentCommander = opponent.Unwrap();
            // Listen for all bases to be defeated.
            playerCommander.AllBasesDefeated += PlayerDefeated;
            opponentCommander.AllBasesDefeated += OpponentDefeated;
        }
        private void PlayerDefeated(Commander player)
        {
            // TODO this is an expensive way to reset scene state,
            // could be more optimal.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void OpponentDefeated(Commander opponent)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion
    }
}
