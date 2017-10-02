using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading;
using Android.Media;

namespace KitchenTimer
{
    [Activity(Label = "KitchenTimer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Timer _timer;
        private int _remainingMillisec = 0;

        private bool _isStart = false;
        private Button _startButton;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);

            var add10MinButton = FindViewById<Button>(Resource.Id.Add10MinButton);
            var add1MinButton = FindViewById<Button>(Resource.Id.Add1MinButton);
            var add10SecButton = FindViewById<Button>(Resource.Id.Add10SecButton);
            var add1SecButton = FindViewById<Button>(Resource.Id.Add1SecButton);
            var clearButton = FindViewById<Button>(Resource.Id.ClearButton);
            _startButton = FindViewById<Button>(Resource.Id.StartButton);
            _timer = new Timer(Timer_OnTick, null, 0, 100);


            add10MinButton.Click += Add10MinButton_Click;
            add1MinButton.Click += (sender, eventargs) => {
                _remainingMillisec += 60 * 1000;
            };
            add10SecButton.Click += (sender, eventargs) => {
                _remainingMillisec += 10 * 1000;
            };
            add1SecButton.Click += (sender, eventargs) => {
                _remainingMillisec += 1 * 1000;
            };
            clearButton.Click += (sender, eventargs) => {
                _isStart = false;
                _remainingMillisec = 0;
                ShowRemainingTime();

            };
            _startButton.Click += (sender, eventargs) =>
            {
                _isStart = !_isStart;
                if (_isStart)
                {
                    _startButton.Text = "ストップ";
                }
                else
                {
                    _startButton.Text = "スタート";
                }
            };

        }

        private void Timer_OnTick(object state)
        {
            if (_isStart)
            {
                return;
            }
            RunOnUiThread(() =>
            {
                _remainingMillisec -= 100;
                if(_remainingMillisec <= 0)
                {
                    _isStart = false;
                    _remainingMillisec = 0;
                    _startButton.Text = "スタート";
                    var toneGenerator = new ToneGenerator(Stream.System, 50);
                    toneGenerator.StartTone(Tone.CdmaHighSs);
                    toneGenerator.Release();
                }
                ShowRemainingTime();
            });
        }

        private void Add10MinButton_Click(object sender, System.EventArgs e)
        {
            _remainingMillisec += 600 * 1000;
            ShowRemainingTime();

        }

        private void ShowRemainingTime()
        {
            var sec = _remainingMillisec / 1000;
            FindViewById<TextView>(Resource.Id.RemainingTimeTextView).Text
                = string.Format("{0:f0}:{1:d2}", sec / 60, sec % 60);
        }
    }
}

