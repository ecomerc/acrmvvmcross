﻿using System;


namespace Acr.MvvmCross.Plugins.DeviceInfo {
    
    public interface IDeviceInfoService {

        int ScreenHeight { get; }
        int ScreenWidth { get; }
        string Manufacturer { get; }
        string Model { get; }
        string OperatingSystem { get; }
    }
}
