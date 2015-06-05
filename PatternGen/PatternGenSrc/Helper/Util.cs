using System;
using Android.Net;
using System.Collections.Generic;
using Android.Graphics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;

namespace PatternGen
{
	public class Util
	{

		private static List<Color> _colorOfflineList;
		private static List<Color> _colorOnlineList;
		static Random rand = new Random ();

		public Util ()
		{
			
		}

		public static bool drawSquare ()
		{
			rand = new Random ();
			if (rand.Next (0, 2) == 0)
				return true;
			else
				return false;

		}

		public static void populateOfflineColorList ()
		{
			_colorOfflineList = new List<Color> ();
			_colorOfflineList.Add (Color.Black);
			_colorOfflineList.Add (Color.BlueViolet);
			_colorOfflineList.Add (Color.Gold);
			_colorOfflineList.Add (Color.Yellow);
			_colorOfflineList.Add (Color.Red);
		}

		public static List<Color> getOfflineColorList ()
		{
			return _colorOfflineList;
		}

		public static List<Color> getOnlineColorList ()
		{
			return _colorOnlineList;
		}

		public static void populateOnlineColorList (List<Color> list)
		{
			_colorOnlineList = list;
		}

		public static Color getRandomColor (List<Color> colorList)
		{

			if (colorList != null && colorList.Count > 0) {
				int r = rand.Next (colorList.Count);
				return colorList [r];
			} else {
				return Color.White;
			}
			
		}

		public static bool isConnectedToNetwork (Android.Content.Context context)
		{
			ConnectivityManager cm = (ConnectivityManager)context.ApplicationContext.GetSystemService (Android.Content.Context.ConnectivityService);
			var activeConnection = cm.ActiveNetworkInfo;
			if (activeConnection != null && activeConnection.IsConnected) {
				return true;
			} else {
				return false;
			}
		}

		public static async Task GetPatternAsync ()
		{
			List<Color> colorList = new List<Color> ();
			RGB rgb = null;

			using (var httpClient = new HttpClient ()) {

				for (int apiCallCount = 0; apiCallCount < Constants.ApiContiniousCallCount; apiCallCount++) {

					Task<string> retrievePatternTask = httpClient.GetStringAsync (Constants.ColorUrl);

					string patternXml = await retrievePatternTask;
					XmlDocument xmlDoc = new XmlDocument ();
					try {
						xmlDoc.LoadXml (patternXml);

						string xpath = Constants.RgbElementPath;
						var nodes = xmlDoc.SelectNodes (xpath);

						rgb = new RGB ();
						var rgbNode = nodes [0];

						rgb.Red = int.Parse (rgbNode.SelectSingleNode (Constants.RedElementPath).InnerText.Trim ());
						rgb.Blue = int.Parse (rgbNode.SelectSingleNode (Constants.GreenElementPath).InnerText.Trim ());
						rgb.Green = int.Parse (rgbNode.SelectSingleNode (Constants.RedElementPath).InnerText.Trim ());

						colorList.Add (new Color ((byte)rgb.Red, (byte)rgb.Blue, (byte)rgb.Green));
					} catch (Exception e) {
						System.Console.WriteLine ("Load error");
					}

				}

			}

			populateOnlineColorList (colorList); // Task<TResult> returns an object of type TResult
		}
			
	}

	public class RGB
	{
		public int Red {
			get;
			set;
		}

		public int Blue {
			get;
			set;
		}

		public int Green {
			get;
			set;
		}
	}
}

