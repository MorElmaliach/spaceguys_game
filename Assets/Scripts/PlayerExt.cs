using Photon.Pun;
using Photon.Realtime;

namespace Utilities
{
	public static class PlayerExt
	{
		
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