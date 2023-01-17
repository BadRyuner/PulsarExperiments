using PulsarExperiments.Features.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulsarExperiments
{
	public class Commands : PulsarModLoader.Chat.Commands.CommandRouter.ChatCommand
	{
		public override string[] CommandAliases() => new string[] { "test" };

		public override string Description() => "test";

		public override void Execute(string arguments)
		{
			switch(arguments)
			{
				case "katana":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("PoorKatana", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);

					}
					break;
				case "fireaxe":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("FireAxe", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);
					}
					break;
				case "eagle":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("SpaceEagle", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);
					}
					break;
				case "t":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("Engineer Tablet", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);
					}
					break;
				case "ufo":
					{
						var ufoid = (EShipType)UfoShip.UFOShipType;
						PLEncounterManager.Instance.GetCPEI().SpawnEnemyShip(ufoid, new PLPersistantShipInfo(ufoid, 2, PLServer.GetCurrentSector()));
					}
					break;
				case "unload":
					Prefabs.bundle.Unload(true);
					break;
				case "load":
					Prefabs.LoadPrefabs();
					break;
				default:
					PulsarModLoader.Utilities.Messaging.Notification($"Unknown subcommand: {arguments}");
					break;
			}
		}
	}
}
