#pragma once

#include "pch.h"
#include "..\TravelMate.NativeDLL\WeatherNative.h"
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace msclr::interop;

namespace TravelMate {
    namespace Interop {

        // Managed weather data class
        public ref class ManagedWeatherData {
        public:
            property float MaxTemperature;
            property float MinTemperature;
            property String^ Description;
            property String^ Wind;
            property String^ Date;
            property String^ Location;
            property bool Success;
            property String^ ErrorMessage;

            ManagedWeatherData() {
                Description = "";
                Wind = "";
                Date = "";
                Location = "";
                ErrorMessage = "";
                Success = false;
                MaxTemperature = 0.0f;
                MinTemperature = 0.0f;
            }
        };

        // Managed reminder data class
        public ref class ManagedReminderData {
        public:
            property int Id;
            property String^ Time;
            property String^ Location;
            property int UserId;
            property String^ Date;

            ManagedReminderData() {
                Time = "";
                Location = "";
                Date = "";
                Id = 0;
                UserId = 0;
            }
        };

        // C++/CLI interop class
        public ref class WeatherInterop {
        public:
            static ManagedWeatherData^ GetWeatherInfo(String^ location, String^ date);
            static bool ValidateReminderData(ManagedReminderData^ reminder);
            static int CalculateReminderHash(ManagedReminderData^ reminder);
            static String^ FormatWeatherDescription(String^ description);
            static ManagedWeatherData^ ParseWeatherJson(String^ jsonResponse, String^ targetDate);
        };

    }
}