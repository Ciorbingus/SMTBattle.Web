using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMTBattle.Web.Services;

public class PresenceCircuitHandler : CircuitHandler
{
    private readonly UserPresenceService _presenceService;
    private readonly AuthenticationStateProvider _authStateProvider;
    private string? _currentUserId;

    public PresenceCircuitHandler(UserPresenceService presenceService, AuthenticationStateProvider authStateProvider)
    {
        _presenceService = presenceService;
        _authStateProvider = authStateProvider;
    }

    public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            _currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = user.Identity.Name ?? "Unknown Tamer";
            
            if (_currentUserId != null)
            {
                _presenceService.SetUserOnline(_currentUserId, username);
            }
        }
        await base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        if (_currentUserId != null)
        {
            _presenceService.SetUserOffline(_currentUserId);
        }
        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    }
}