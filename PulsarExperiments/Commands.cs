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
				case "1":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("PoorKatana", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);

					}
					break;
				case "2":
					{
						PulsarModLoader.Content.Items.ItemModManager.Instance.GetItemIDsFromName("SpaceEagle", out int Main, out int Sub);
						PLNetworkManager.Instance.LocalPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, Main, Sub, 0, -1);
					}
					break;
				case "3":
					{
						var ufoid = (EShipType)UfoShip.UFOShipType;
						PLEncounterManager.Instance.GetCPEI().SpawnEnemyShip(ufoid, new PLPersistantShipInfo(ufoid, 2, PLServer.GetCurrentSector()));
					}
					break;
			}
		}
	}
}
