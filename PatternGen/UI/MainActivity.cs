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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			LinearLayout m_Layout = FindViewById<LinearLayout> (Resource.Id.myLayout);
			GestureRecognizerView gv = FindViewById<GestureRecognizerView>(Resource.Id.myGestureCustomView);
		}

		public bool OnTouch(View v, MotionEvent e) {


			switch (e.Action)
			{
			case MotionEventActions.Down:
				mX = e.GetX();
				mY = e.GetY();
				break;
			case MotionEventActions.Move:
				//var left = (int) (e.RawX - _viewX);
				//var right = (int) (left + v.Width);
				//v.Layout(left, v.Top, right, v.Bottom);
				break;
			}
			return true;

		}

	}
}


