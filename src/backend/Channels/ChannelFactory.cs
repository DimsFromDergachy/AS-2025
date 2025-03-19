using System.Threading.Channels;
namespace AS_2025.Channels;

public class ChannelFactory
{
    public static Channel<ApiEvent> CreateApiEventsChannel()
    {
        return Channel.CreateUnbounded<ApiEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });
    }
}