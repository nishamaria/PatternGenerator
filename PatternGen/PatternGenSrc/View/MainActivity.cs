using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using System.Collections.Generic;
using Android.Graphics;

namespace PatternGen
{
	[Activity (Label = "PatternGen", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		PatternGeneratorView patternGenView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			LinearLayout m_Layout = FindViewById<LinearLayout> (Resource.Id.myLayout);
			patternGenView = FindViewById<PatternGeneratorView> (Resource.Id.myGestureCustomView);

		}

		protected override void OnResume ()
		{
			base.OnResume ();
			if (Util.isConnectedToNetwork (this)) {
				
				Task sizeTask = Util.GetPatternAsync ();
			} 
			Util.populateOfflineColorList ();

		}

	}
}


