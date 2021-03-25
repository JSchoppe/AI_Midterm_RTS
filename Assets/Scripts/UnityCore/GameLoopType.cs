namespace AI_Midterm_RTS.UnityCore
{
    /// <summary>
    /// Enum for the loop types implemented in Unity.
    /// </summary>
    public enum GameLoopType : byte
    {
        /// <summary>
        /// The update loop tied to framerate.
        /// </summary>
        Update,
        /// <summary>
        /// The update loop tied to physics.
        /// </summary>
        FixedUpdate
    }
}
