namespace PatternGen
{
	using Android.Content;
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.Util;
	using Android.Views;

	/// <summary>
	///   This class will show how to respond to touch events using a custom subclass
	///   of View.
	/// </summary>
	public class GestureRecognizerView : View
	{
		private static readonly int InvalidPointerId = -1;
		private Drawable _icon;
		private int _activePointerId = InvalidPointerId;
		private float _lastTouchX;
		private float _lastTouchY;
		private float _posX;
		private float _posY;
		private float _scaleFactor = 1.0f;
		private Paint _mPaint;
		private bool _mTouched;
		private Context _mContext;
		private bool _mDrawSquare;

		public GestureRecognizerView (Context context)
			: base (context, null, 0)
		{
			_mContext = context;
			_icon = context.Resources.GetDrawable (PatternGen.Resource.Drawable.ic_launcher);
			_icon.SetBounds (0, 0, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
			_mPaint = new Paint ();
		}

		public GestureRecognizerView (Context context, IAttributeSet attrs) :
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
				_mDrawSquare = Util.drawSquare ();
				_lastTouchX = ev.GetX ();
				_lastTouchY = ev.GetY ();
				_activePointerId = ev.GetPointerId (0);
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
			System.Console.WriteLine ("onDraw");
			base.OnDraw (canvas);
			canvas.Save ();
			canvas.Translate (_posX, _posY);
			canvas.Scale (_scaleFactor, _scaleFactor);
			//_icon.Draw (canvas);
			_mPaint.SetARGB (95, 28, 53, 0);
			if (_mTouched) {
				if (Util.isConnectedToNetwork (_mContext)) {
					_mPaint.SetARGB (100, 30, 20, 0);
					canvas.DrawCircle (_lastTouchX, _lastTouchY, 50, _mPaint);
				} else {
					if (Util.drawSquare ()) {
						_mPaint.SetARGB (200, 100, 200, 0);
						canvas.DrawCircle (_lastTouchX, _lastTouchY, 50, _mPaint);
						//_icon.SetBounds ((int)_lastTouchX, (int)_lastTouchY, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
						//_icon.Draw (canvas);
					} else {
						_mPaint.SetARGB (95, 28, 53, 0);
						canvas.DrawCircle (_lastTouchX, _lastTouchY, 50, _mPaint);
					}
				}
			}
			canvas.Restore ();
		}
			
	}
}
