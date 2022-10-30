namespace Carnets.Application.Consts
{
    public static class CarnetsConsts
    {
        #region Keys

        public const string GympassIdKey = "gympassId";

        public const string EntryTokenValidityInSecondsKey = "EntryTokenValidityInSeconds";

        #endregion

        #region Payload

        public const string GympassIdPayloadKey = "gympassId";

        public const string ExpiresDatePayloadKey = "expires";

        #endregion

        #region Validation

        public const string GympassNotActive = "gympass_not_active";

        public const string GympassValidityEnded = "gympass_validity_ended";

        public const string GympassNoMoreEntries = "gympass_no_more_entries";

        public const string GympassEntryMinuteNotAllowed = "gympass_entry_minute_not_allowed";

        #endregion
    }
}
