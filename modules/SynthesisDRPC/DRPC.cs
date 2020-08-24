using DiscordRPC;
using DiscordRPC.Message;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.Utilities;

namespace DRPC
{
	public class SynthesisDRPC : SystemBase
	{
		public DiscordRpcClient client;

		public override void OnPhysicsUpdate()
		{
			//throw new System.NotImplementedException();
		}

		public override void Setup()
		{
			client = new DiscordRpcClient("738906761733603428");
			client.Initialize();

			//Subscribe to events
			client.OnConnectionFailed += (sender, e) => Logger.Log("There was an error while trying to connect to the rich presence.");
			client.OnConnectionEstablished += (sender, e) => Logger.Log("Rich presence connected.");
			client.OnError += (sender, e) => Logger.Log("Rich presence error: " + e.Message);
			client.OnReady += (sender, e) => Logger.Log("Rich presence ready.");

			Logger.Log("start");

			client.SetPresence(new RichPresence()
			{
				Details = "An Autodesk Technology",
				Timestamps = Timestamps.Now,
				//State = "robot thing",
				Assets = new Assets()
				{
					LargeImageKey = "w16_syn_launch",
					LargeImageText = "Synthesis",
					SmallImageKey = "image_small"
				}
			});
			Logger.Log("end");
		}

		public override void OnUpdate()
		{
			client.Invoke();
		}

		public override void Teardown()
		{
			Logger.Log("halted!");
			client.Dispose();
		}
	}
}
