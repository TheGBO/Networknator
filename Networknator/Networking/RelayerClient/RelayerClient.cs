using System;
using System.Net;
using Newtonsoft.Json;

namespace Networknator.Networking.RelayerClient
{
    public class RelayerClient
    {
        private int RelayerTcpPort { get; set; }
        private string ApiAddress { get; set; }

        public RelayerClient(int relayerTcpPort, string apiAddress)
        {
            RelayerTcpPort = relayerTcpPort;
            ApiAddress = apiAddress;

        }

        public async void CreateRoom(bool publicRoom, Action<RoomData> callback)
        {
            using(WebClient wc = new WebClient())
            {
                string roomDataJson = JsonConvert.SerializeObject(new RoomData { PublicRoom = publicRoom });
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                string response = await wc.UploadStringTaskAsync(new Uri("http://" + ApiAddress + "/room"), roomDataJson);
                callback(JsonConvert.DeserializeObject<RoomData>(response));
            }
        }

        public async void JoinRoom(string joinCode, Action<RoomData> callback)
        {
            using (WebClient wc = new WebClient())
            {
                string roomDataJson = JsonConvert.SerializeObject(new RoomData { JoinCode = joinCode });
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                string response = await wc.UploadStringTaskAsync(new Uri("http://" + ApiAddress + "/joinroom"), roomDataJson);
                RoomData data = JsonConvert.DeserializeObject<RoomData>(response);
                callback(data);
            }
        }
    }
}