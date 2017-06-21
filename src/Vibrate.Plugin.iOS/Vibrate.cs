using System;
using Plugin.Vibrate.Abstractions;
using AudioToolbox;


namespace Plugin.Vibrate
{
    /// <summary>
    /// iOS implementation to vibrate device
    /// </summary>
    public class Vibrate : IVibrate
    {
        /// <summary>
        /// Vibrate the phone for specified amount of time
        /// </summary>
        /// <param name="vibrateSpan">Time span to vibrate. 500ms is default if null</param>
        public void Vibration(TimeSpan? vibrateSpan = null) =>
            SystemSound.Vibrate.PlaySystemSound();
        
    }
}