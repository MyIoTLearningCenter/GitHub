using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.Devices.Gpio;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BlinkyHeadless2
{
    public sealed class StartupTask : IBackgroundTask
    {

            BackgroundTaskDeferral _deferral;
        private const int LED_PIN = 5;
        private GpioPin pin;
        private GpioPinValue pinValue;
        private ThreadPoolTimer timer;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            InitGPIO();

            timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick,
                       TimeSpan.FromMilliseconds(500));
        }

        private void Timer_Tick(ThreadPoolTimer timer)
        {
            if (GpioPinValue.High == pinValue)
                pinValue = GpioPinValue.Low;
            else
                pinValue = GpioPinValue.High;

            pin.Write(pinValue);
        }

        private void InitGPIO()
        {
            GpioController gpio = GpioController.GetDefault();
            pin = gpio.OpenPin(LED_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);


        }
    }
}
