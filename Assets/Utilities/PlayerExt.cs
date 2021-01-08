using Photon.Pun;
using Photon.Realtime;

namespace Utilities
{
	public static class PlayerExt
	{
		
#region PlayerReadyState

		public static void SetReady(this Player player, bool value)
		{
			player.SetPropertyValue(PlayerProperties.Ready, value);
		}

		public static bool IsReady(this Player player)
		{
			return player.GetPropertyValue(PlayerProperties.Ready, false);
		}

#endregion
		
#region Score

		public static void SetScore(this Player player, int amount)
		{
			player.SetPropertyValue(PlayerProperties.Score, amount);
		}

		public static int GetScore(this Player player)
		{
			return player.GetPropertyValue(PlayerProperties.Score, 0);
		}
		
		public static void AddScore(this Player player, int amount)
		{
			player.AddValueToProperty(PlayerProperties.Score, amount);
		}
		#endregion
	}
}