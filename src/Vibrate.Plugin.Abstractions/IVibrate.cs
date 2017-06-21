using System;

namespace Plugin.Vibrate.Abstractions
{
    /// <summary>
    /// Vibration interface
    /// </summary>
    public interface IVibrate
    {
        /// <summary>
        /// Vibrate the phone for specified amount of time
        /// </summary>
        /// <param name="vibrateSpan">Time span to vibrate. 500ms is default if null</param>
        void Vibration(TimeSpan? vibrateSpan = null);
    }
}
