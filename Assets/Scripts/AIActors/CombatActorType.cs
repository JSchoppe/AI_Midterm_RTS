namespace AI_Midterm_RTS.AIActors
{
    /// <summary>
    /// Identifies the subclass of the combat actor.
    /// </summary>
    public enum CombatActorType : byte
    {
        /// <summary>
        /// A MeleeCombatActor.
        /// </summary>
        Melee,
        /// <summary>
        /// A RangedCombatActor.
        /// </summary>
        Ranged,
        /// <summary>
        /// A JoustingCombatActor.
        /// </summary>
        Jousting,
        /// <summary>
        /// An AOECombatActor.
        /// </summary>
        AOE
    }
}
