using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Enums
{
    public enum TournamentType
    {
        [EnumMember(Value = "single elimination")]
        SingleElimination,
        [EnumMember(Value = "double elimination")]
        DoubleElimination,
        [EnumMember(Value = "round robin")]
        RoundRobin,
        [EnumMember(Value = "swiss")]
        Swiss,
        [EnumMember(Value = "free for all")]
        FreeForAll,
        [EnumMember(Value = "leaderboard")]
        Leaderboard,
        [EnumMember(Value = "time trial")]
        TimeTrial,
        [EnumMember(Value = "single race")]
        SingleRace,
        [EnumMember(Value = "grand prix")]
        GrandPrix
    }
}
