namespace FocusTrack.API.DTO
{
    public record CreateSessionRequest(
     string Topic,
     DateTimeOffset StartTime,
     DateTimeOffset EndTime,
     string Mode
 );
}
