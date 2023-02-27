using LANPartyAPI_Core.Models;

namespace LANPartyAPI_Core.Enums
{
    public static class ExceptionErrorMessages
    {
        public static class EventExceptions
        {
            public const string EventNotFound = $"{nameof(Event)}NotFound";
            public const string EventNameTaken = $"{nameof(Event)}NameTaken";
            public const string EventNameRequired = $"{nameof(Event)}NameRequired";
            public const string EventStartDateRequired = $"{nameof(Event)}StartDateRequired";
            public const string EventEndDateRequired = $"{nameof(Event)}EndDateRequired";
            public const string EventInvalidDates = $"{nameof(Event)}InvalidDates";
            public const string EventMaxPlayersNumberNotInRange = $"{nameof(Event)}MaxPlayersNumberNotInRange";
			public const string EventMaxPlayersNumberReached = $"{nameof(Event)}MaxPlayersNumberReached";
            public const string EventNameTooLong = $"{nameof(Event)}NameTooLong";
            public const string EventDescriptionTooLong = $"{nameof(Event)}DescriptionTooLong";

        }

        public static class TournamentExceptions
        {
            public const string TournamentNotFound = $"{nameof(Tournament)}NotFound";
            public const string TournamentNameTaken = $"{nameof(Tournament)}NameTaken";
            public const string TournamentNameRequired = $"{nameof(Tournament)}NameRequired";
            public const string TournamentPlayersPerTeamNumberNotInRange = $"{nameof(Tournament)}PlayersPerTeamNumberNotInRange";
            public const string TournamentMaxTeamNumberNotInRange = $"{nameof(Tournament)}MaxTeamNumberNotInRange";
            public const string TournamentGameRequired = $"{nameof(Tournament)}GameRequired";
            public const string TournamentEliminationModeRequired = $"{nameof(Tournament)}EliminationModeRequired";
            public const string TournamentEliminationModeNotInRange = $"{nameof(Tournament)}EliminationModeNotInRange";
            public const string TournamentEventIdRequired = $"{nameof(Tournament)}EventIdRequired";
            public const string TournamentNameTooLong = $"{nameof(Tournament)}NameTooLong";
            public const string TournamentDescriptionTooLong = $"{nameof(Tournament)}DescriptionTooLong";
            public const string TournamentGameTooLong = $"{nameof(Tournament)}GameTooLong";

        }

        public static class MatchExceptions
        {
            public const string MatchNotFound = $"{nameof(Match)}NotFound";
            public const string MatchInvalidStartDate = $"{nameof(Match)}InvalidStartDate";
            public const string MatchAlreadyExist = $"{nameof(Match)}AlreadyExist";
            public const string MatchRoundRequired = $"{nameof(Match)}RoundRequired";
            public const string MatchNumberRequired = $"{nameof(Match)}NumberRequired";
            public const string MatchTournamentIdRequired = $"{nameof(Match)}TournamentIdRequired";
            public const string MatchRoundInvalid = $"{nameof(Match)}RoundInvalid";
            public const string MatchNumberInvalid = $"{nameof(Match)}NumberInvalid";

            public const string MatchResultInvalidScore = $"{nameof(Match)}ResultInvalidScore";

        }

        public static class TeamExceptions
        {
            public const string TeamNotFound = $"{nameof(Team)}NotFound";
            public const string TeamAlreadySetInRound = $"{nameof(Team)}AlreadySetInRound";
            public const string TeamNameRequired = $"{nameof(Team)}NameRequired";
            public const string TeamNameTooLong = $"{nameof(Team)}NameTooLong";

            public const string TeamLimitReached = $"{nameof(Team)}LimitReached";
            public const string TeamPlayersLimitReached = $"{nameof(Team)}PlayersLimitReached";
            public const string TeamNameAlreadyTaken = $"{nameof(Team)}NameAlreadyTaken";

        }

 

		public static class UserExceptions
		{
			public const string UserNotFound = $"{nameof(ApplicationUser)}NotFound";
			public const string UserAlreadyJoinedEvent = $"{nameof(ApplicationUser)}AlreadyJoined";
            public const string UserNotInEvent = $"{nameof(ApplicationUser)}NotInEvent";
            public const string UserNotOwnerOfSeat = $"{nameof(ApplicationUser)}NotOwnerOfSeat";

            public const string UserAlreadyJoinedTournament = $"{nameof(ApplicationUser)}AlreadyJoinedTournament";
            public const string UserNotInTournament = $"{nameof(ApplicationUser)}NotInTournament";
        }

    }
}
