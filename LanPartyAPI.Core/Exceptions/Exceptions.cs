using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Models;

namespace LANPartyAPI_Core.Exceptions
{
    #region EventExceptions

    public class EventNotFoundException : Exception { }
    public class EventNameTakenException : Exception { }
    public class EventInvalidDatesException : Exception { }
    public class EventMaxPlayersNumberReachedException : Exception { }

    #endregion

    #region TournamentExceptions

    public class TournamentNotFoundException : Exception { }
    public class TournamentNameTakenException : Exception { }


    #endregion

    #region MatchExceptions

    public class MatchNotFoundException : Exception { }
    public class MatchInvalidStartDateException : Exception { }
    public class MatchAlreadyExistException : Exception { }

    #endregion

    #region TeamExceptions

    public class TeamNotFoundException : Exception { }
    public class TeamAlreadySetInRoundException : Exception { }
    public class TeamLimitReachedException : Exception { } //todo add
    public class TeamPlayersLimitReachedException : Exception { } //todo add
    public class TeamNameAlreadyTakenException : Exception { } //todo add

    #endregion

    #region SeatExceptions

    public class SeatNotFoundException : Exception { }
    public class SeatAlreadyExistException : Exception { }
    public class SeatDuplicateIdException : Exception { }
    public class SeatDuplicatePositionException : Exception { }
    public class SeatAlreadyReservedException : Exception { }
    public class NoSeatsGivenException : Exception { }

    #endregion

    #region PictureExceptions

    public class PictureNotFoundException : Exception { }
    public class NoFileGivenException : Exception { }

    #endregion

    #region UserExceptions

    public class UserNotFoundException : Exception { }
    public class UserAlreadyJoinedEventException : Exception { } 
    public class UserAlreadyJoinedTournamentException : Exception { } // todo:
    public class UserNotInEventException : Exception { }
    public class UserNotInTournamentException : Exception { } //todo
    public class UserNotOwnerOfSeatException : Exception { }

    #endregion
}
