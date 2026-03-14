namespace SMTBattle.Web.Services;

public class UserPresenceService
{
    private readonly Dictionary<string, string> _onlineUsers = new();
    public event Action? OnPresenceChanged;

    public void SetUserOnline(string userId, string username)
    {
        lock (_onlineUsers)
        {
            _onlineUsers[userId] = username;
        }
        OnPresenceChanged?.Invoke();
    }

    public void SetUserOffline(string userId)
    {
        lock (_onlineUsers)
        {
            if (_onlineUsers.Remove(userId))
            {
                OnPresenceChanged?.Invoke();
            }
        }
    }

    public int GetActiveUsersCount() => _onlineUsers.Count;

    public List<string> GetOnlineUsernames()
    {
        lock (_onlineUsers)
        {
            return _onlineUsers.Values.ToList();
        }
    }
}