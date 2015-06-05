using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PatternGen
{
	using Android.Content;
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.Util;
	using Android.Views;

	public class PatternGeneratorView : View
	{
		private static readonly int InvalidPointerId = -1;
		private Drawable _icon;
		private int _activePointerId = InvalidPointerId;
		private float _lastTouchX;
		private float _lastTouchY;
		private float _posX;
		private float _posY;
		private Paint _mPaint;
		private bool _mTouched;
		private Context _mContext;

		public PatternGeneratorView (Context context)
			: base (context, null, 0)
		{
			_mContext = context;
			_icon = context.Resources.GetDrawable (PatternGen.Resource.Drawable.ic_launcher);
			_icon.SetBounds (0, 0, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
			_mPaint = new Paint ();
		}

		public PatternGeneratorView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			_mContext = context;
			_icon = context.Resources.GetDrawable (PatternGen.Resource.Drawable.ic_launcher);
			_icon.SetBounds (0, 0, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
			_mPaint = new Paint ();
		}


		public override bool OnTouchEvent (MotionEvent ev)
		{
			
			MotionEventActions action = ev.Action & MotionEventActions.Mask;
			int pointerIndex;

			switch (action) {
			case MotionEventActions.Down:
				_mTouched = true;
				_lastTouchX = ev.GetX ();
				_lastTouchY = ev.GetY ();
				_activePointerId = ev.GetPointerId (0);

				//Keep populating the online list
				Task sizeTask = Util.GetPatternAsync ();
			
				break;

			case MotionEventActions.Move:
				pointerIndex = ev.FindPointerIndex (_activePointerId);
				float x = ev.GetX (pointerIndex);
				float y = ev.GetY (pointerIndex);

				float deltaX = x - _lastTouchX;
				float deltaY = y - _lastTouchY;
				_posX += deltaX;
				_posY += deltaY;
				Invalidate ();

				_lastTouchX = x;
				_lastTouchY = y;

				break;

			case MotionEventActions.Up:
			case MotionEventActions.Cancel:
                    // This events occur when something cancels the gesture (for example the
                    // activity going in the background) or when the pointer has been lifted up.
                    // We no longer need to keep track of the active pointer.
				_activePointerId = InvalidPointerId;
				break;

			case MotionEventActions.PointerUp:
                    // We only want to update the last touch position if the the appropriate pointer
                    // has been lifted off the screen.
				pointerIndex = (int)(ev.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
				int pointerId = ev.GetPointerId (pointerIndex);
				if (pointerId == _activePointerId) {
					// This was our active pointer going up. Choose a new
					// action pointer and adjust accordingly
					int newPointerIndex = pointerIndex == 0 ? 1 : 0;
					_lastTouchX = ev.GetX (newPointerIndex);
					_lastTouchY = ev.GetY (newPointerIndex);
					_activePointerId = ev.GetPointerId (newPointerIndex);
				}
				break;
			}
			return true;
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);

			//TODO: Handle mutiple calls of onDraw
			canvas.Save ();
			canvas.Translate (_posX, _posY);
			//_icon.Draw (canvas);
			_mPaint.SetARGB (31, 23, 15, 7);
			if (_mTouched) {
				
				//TODO: Generate pattern from imageurl

				// This just get the color based on online and offline mode
				if (Util.isConnectedToNetwork (_mContext)) {
					if (Util.getOnlineColorList () != null && Util.getOnlineColorList ().Count > 0) {
						Color color = Util.getOnlineColorList () [0];
						_mPaint.Color = color;
						Util.getOnlineColorList ().Remove (color);
					}
					drawShape (_mPaint, canvas);
				} else {
					_mPaint.Color = Util.getRandomColor (Util.getOfflineColorList ());
					drawShape (_mPaint, canvas);
				}
			}
			canvas.Restore ();
		}

		private void drawShape (Paint paint, Canvas canvas)
		{
			if (Util.drawSquare ()) {
				canvas.DrawRect (_lastTouchX, _lastTouchY, _lastTouchX + 70, _lastTouchY + 70, paint);
			} else {
				canvas.DrawCircle (_lastTouchX, _lastTouchY, 50, _mPaint);

			}
		}
			
	}

}
