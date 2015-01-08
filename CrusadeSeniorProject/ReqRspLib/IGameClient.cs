namespace ReqRspLib
{
    public interface IGameClient
    {
        // don't want to make a reference to game library so just using string
        string PlayerNumber { get; set; }
    }
}
