using System.Net;
using System.Net.Sockets;
using System.Text;

namespace dualsense_support;

public class DsxUdpClient
{
    private readonly UdpClient _client;

    private readonly IPEndPoint _endPoint;

    public DsxUdpClient()
    {
        _client = new UdpClient();
        var portNumber = File.ReadAllText(@"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt");
        // portNumber = "6970";
        _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Convert.ToInt32(portNumber));
    }

    public void Send(string packet)
    {
        if(packet == "") return;
        
        var requestData = Encoding.ASCII.GetBytes(packet);
        _client.Send(requestData, requestData.Length, _endPoint);
    }
}