namespace AI_Midterm_RTS.Commanders
{
    /// <summary>
    /// A commander that is controlled by a player.
    /// </summary>
    public sealed class PlayerCommander : Commander
    {
        #region Constructors
        /// <summary>
        /// Creates a new player commander given
        /// the team ID.
        /// </summary>
        /// <param name="teamID">Controls how this player's units attack others.</param>
        public PlayerCommander(byte teamID) : base(teamID)
        {
            // TODO maybe this class should
            // hold references to the cursor?
        }
        #endregion
    }
}
