using System.Net;
using System.Net.Sockets;
using System.Text;

namespace dualsense_support;

public class DsxUdpClient
{
    private readonly UdpClient _client;

    private readonly IPEndPoint _endPoint;

    private const string PortFilePath = @"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt";

    public DsxUdpClient()
    {
        _client = new UdpClient();
        if (!File.Exists(PortFilePath))
        {
            throw new Exception($"DSX Port file doesn't exist. Missing: {PortFilePath}");
        }

        var portNumber = File.ReadAllText(PortFilePath);
        // portNumber = "6970";
        _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Convert.ToInt32(portNumber));
    }

    public void Send(string packet)
    {
        if (packet == "") return;

        var requestData = Encoding.ASCII.GetBytes(packet);
        _client.Send(requestData, requestData.Length, _endPoint);
    }
}