using System;
using System.Threading;
using System.Threading.Tasks;
using Open.Nat;

namespace OpenBitcoin8333Port
{
    class Program
    {
        static void Main()
        {
            try
            {
                DoIt().Wait();
                Console.WriteLine("Done!");
            }
            catch(AggregateException e)
            {
                var nfe = e.InnerException as NatDeviceNotFoundException; 
                if(nfe != null)
                {
                    Console.WriteLine("No NAT device was found.");
                    return;
                }
                throw;
            }
        }

        private static async Task DoIt()
        {
            var discover = new NatDiscoverer();
            var device = await discover.DiscoverDeviceAsync(PortMapper.Upnp, new CancellationTokenSource(5000));

            var mapping = new Mapping(Protocol.Tcp, 8333, 8333, 360*24, "Bitcoin Node");
            await device.CreatePortMapAsync(mapping);
        }
    }
}
