using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace PatternGen
{
	[Activity (Label = "PatternGen", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		float mX;
		float mY;
		GestureRecognizerView gv;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			LinearLayout m_Layout = FindViewById<LinearLayout> (Resource.Id.myLayout);
			gv = FindViewById<GestureRecognizerView>(Resource.Id.myGestureCustomView);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			new PatternFetchTask(this, gv).Execute();


		}
			
		public bool OnTouch(View v, MotionEvent e) {


			switch (e.Action)
			{
			case MotionEventActions.Down:
				mX = e.GetX();
				mY = e.GetY();
				break;
			case MotionEventActions.Move:
				break;
			}
			return true;

		}


		public class PatternFetchTask : AsyncTask  
		{
			private Context _context;
			private GestureRecognizerView _gv;

			public PatternFetchTask(Context context, GestureRecognizerView gv)
			{
				_context = context;
				_gv = gv;
			}
				
			protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
			{
				PattenFetcher fetcher = new PattenFetcher ();
				return (Java.Lang.Object)fetcher.GetColorHexValue();
			}

			protected override void OnPostExecute(Java.Lang.Object result)
			{
				base.OnPostExecute(result);
				_gv.HexColor = (RGB)result;

			}
		}

	}
}


