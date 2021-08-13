using diga.web.Models.PlatformNotifications;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace diga.web.Models.Hubs
{
    public class HubUserCollection
    {
        public readonly ConcurrentDictionary<int, List<HubUser>> Connections =
            new ConcurrentDictionary<int, List<HubUser>> { };
    }
}
