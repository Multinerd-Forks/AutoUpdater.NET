namespace AutoUpdater.NETStandard
{
    /// <summary>
    ///     Enum representing the beahviour of the update procedure.
    /// </summary>
    public enum UpdateType
	{
        /// <summary>
        ///     In this mode, it will use all available libaray features.
        /// </summary>
        Normal,

        /// <summary>
        ///     In this mode, it ignores Remind Later and Skip values set previously and hide both buttons.
        /// </summary>
        Recommended,

        /// <summary>
        ///     In this mode, it won't show close button in addition to Normal mode behaviour.
        /// </summary>
        Forced,

        /// <summary>
        ///     In this mode, it will start downloading and applying update without showing standarad update dialog in addition to
        ///     Forced mode behaviour.
        /// </summary>
        Critical
	}
}
