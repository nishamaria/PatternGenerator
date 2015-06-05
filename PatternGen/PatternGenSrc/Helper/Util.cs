using System;
using Android.Net;

namespace PatternGen
{
	public class Util
	{
		public Util ()
		{
			
		}

		public static bool drawSquare ()
		{
			Random rand = new Random ();
			if (rand.Next (0, 2) == 0)
				return true;
			else
				return false;

		}

		public static bool drawPattern ()
		{
			Random rand = new Random ();
			if (rand.Next (0, 2) == 0)
				return true;
			else
				return false;

		}

		public static bool isConnectedToNetwork(Android.Content.Context context){
			ConnectivityManager cm = (ConnectivityManager)context.ApplicationContext.GetSystemService (Android.Content.Context.ConnectivityService);
			var activeConnection = cm.ActiveNetworkInfo;
			if (activeConnection != null && activeConnection.IsConnected) {
				return true;
			} else {
				return false;
			}
		}
			
	}
}

