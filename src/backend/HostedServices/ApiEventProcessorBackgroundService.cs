using AS_2025.Channels;
using AS_2025.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace AS_2025.HostedServices;

public class ApiEventProcessorBackgroundService : BackgroundService
{
    private readonly Channel<ApiEvent> _channel;
    private readonly IHubContext<ApiEventsHub> _hubContext;
    private readonly ILogger<ApiEventProcessorBackgroundService> _logger;

    public ApiEventProcessorBackgroundService(Channel<ApiEvent> channel, IHubContext<ApiEventsHub> hubContext, ILogger<ApiEventProcessorBackgroundService> logger)
    {
        _channel = channel;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[ApiEventProcessorBackgroundService] started at: {time}", DateTimeOffset.UtcNow);

        await foreach (var apiEvent in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ApiCallEvent", apiEvent, stoppingToken);

                _logger.LogInformation("[ApiEventProcessorBackgroundService] Sent API event `ApiCallEvent`: {ApiEvent}", apiEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ApiEventProcessorBackgroundService] Error processing API event");
            }
        }
    }
}

/*
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/api-events")
    .withAutomaticReconnect()
    .build();

connection.on("ApiCallEvent", (apiEvent) => {
    console.log(`API Event: ${apiEvent.method} ${apiEvent.path} at ${apiEvent.timestamp}`);
});

connection.start().catch(err => console.error(err));
*/