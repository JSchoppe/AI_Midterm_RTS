using AI_Midterm_RTS.AICore;

namespace AI_Midterm_RTS.AIActors.States
{
    /// <summary>
    /// Default state where an actor does nothing.
    /// </summary>
    public sealed class DisabledState : IState
    {
        #region Fields
        private readonly CombatActor actor;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates the disabled state for the given actor.
        /// </summary>
        /// <param name="actor">The actor to effect.</param>
        public DisabledState(CombatActor actor)
        {
            this.actor = actor;
        }
        #endregion
        #region State Implementation
        public void StateEntered()
        {
            // Agent should do nothing in this state.
            actor.Navigator.QuitRoute();
        }
        public void StateExited()
        {
            
        }
        #endregion
    }
}
