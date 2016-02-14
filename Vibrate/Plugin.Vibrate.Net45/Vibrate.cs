using System;
using Plugin.Vibrate.Abstractions;

namespace Plugin.Vibrate
{
    /// <summary>
    /// Implementation to vibrate device
    /// </summary>
    public class Vibrate : IVibrate
    {
        /// <summary>
        /// Vibrate device with default length
        /// </summary>
        /// <param name="milliseconds">Ignored (iOS doesn't expose)</param>
        public void Vibration(int milliseconds = 500)
        {
            
        }
    }
}

