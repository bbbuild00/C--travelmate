#include "pch.h"
#include "WeatherInterop.h"

using namespace TravelMate::Interop;
using namespace System;
using namespace msclr::interop;

// 动态加载原生 DLL 的函数指针
typedef int(*GetWeatherInfoFunc)(const char*, const char*, WeatherData*);
typedef int(*ValidateReminderDataFunc)(const ReminderData*);
typedef int(*CalculateReminderHashFunc)(const ReminderData*);
typedef void(*FormatWeatherDescriptionFunc)(char*, int);
typedef int(*ParseWeatherJsonFunc)(const char*, WeatherData*, const char*);

static HMODULE hNativeDll = nullptr;
static GetWeatherInfoFunc pGetWeatherInfo = nullptr;
static ValidateReminderDataFunc pValidateReminderData = nullptr;
static CalculateReminderHashFunc pCalculateReminderHash = nullptr;
static FormatWeatherDescriptionFunc pFormatWeatherDescription = nullptr;
static ParseWeatherJsonFunc pParseWeatherJson = nullptr;

// 初始化原生 DLL
bool InitializeNativeDll() {
    if (hNativeDll != nullptr) return true;

    // 尝试多个可能的路径
    const char* dllPaths[] = {
        "TravelMate.NativeDll.dll",
        "..\\..\\..\\..\\x64\\Debug\\TravelMate.NativeDll.dll",
        "x64\\Debug\\TravelMate.NativeDll.dll"
    };

    for (const char* path : dllPaths) {
        hNativeDll = LoadLibraryA(path);
        if (hNativeDll != nullptr) break;
    }

    if (hNativeDll == nullptr) {
        // 输出调试信息
        DWORD error = GetLastError();
        char errorMsg[256];
        sprintf_s(errorMsg, "LoadLibrary failed with error code: %d", error);
        OutputDebugStringA(errorMsg);
        return false;
    }

    pGetWeatherInfo = (GetWeatherInfoFunc)GetProcAddress(hNativeDll, "GetWeatherInfo");
    pValidateReminderData = (ValidateReminderDataFunc)GetProcAddress(hNativeDll, "ValidateReminderData");
    pCalculateReminderHash = (CalculateReminderHashFunc)GetProcAddress(hNativeDll, "CalculateReminderHash");
    pFormatWeatherDescription = (FormatWeatherDescriptionFunc)GetProcAddress(hNativeDll, "FormatWeatherDescription");
    pParseWeatherJson = (ParseWeatherJsonFunc)GetProcAddress(hNativeDll, "ParseWeatherJson");

    return (pGetWeatherInfo != nullptr && pValidateReminderData != nullptr &&
        pCalculateReminderHash != nullptr && pFormatWeatherDescription != nullptr &&
        pParseWeatherJson != nullptr);
}

ManagedWeatherData^ WeatherInterop::GetWeatherInfo(String^ location, String^ date) {
    if (location == nullptr || date == nullptr) {
        return nullptr;
    }

    try {
        // 方案A: 使用 C++ DLL 的真实API调用
        if (InitializeNativeDll() && pGetWeatherInfo != nullptr) {
            marshal_context ctx;
            const char* nativeLocation = ctx.marshal_as<const char*>(location);
            const char* nativeDate = ctx.marshal_as<const char*>(date);

            WeatherData weatherData;
            memset(&weatherData, 0, sizeof(WeatherData));
            int result = pGetWeatherInfo(nativeLocation, nativeDate, &weatherData);

            ManagedWeatherData^ managedData = gcnew ManagedWeatherData();
            managedData->MaxTemperature = weatherData.maxTemperature;
            managedData->MinTemperature = weatherData.minTemperature;
            managedData->Description = gcnew String(weatherData.description);
            managedData->Wind = gcnew String(weatherData.wind);
            managedData->Date = gcnew String(weatherData.date);
            managedData->Location = gcnew String(weatherData.location);
            managedData->Success = (weatherData.success == 1);
            managedData->ErrorMessage = gcnew String(weatherData.errorMessage);

            return managedData;
        }

        // 方案B: 如果C++ DLL加载失败，返回错误而不是模拟数据
        ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
        errorData->Success = false;
        errorData->ErrorMessage = "Failed to load native DLL or function - please use C# implementation";
        return errorData;

        /* 旧的模拟数据代码 - 已移除
        ManagedWeatherData^ managedData = gcnew ManagedWeatherData();
        managedData->MaxTemperature = 25.0f;
        managedData->MinTemperature = 15.0f;
        managedData->Description = "晴转多云";
        managedData->Wind = "微风 3级";
        managedData->Date = date;
        managedData->Location = location;
        managedData->Success = true;
        managedData->ErrorMessage = "";
        return managedData;
        */
    }
    catch (const std::exception& e) {
        ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
        errorData->Success = false;
        errorData->ErrorMessage = gcnew String(e.what());
        return errorData;
    }
    catch (...) {
        ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
        errorData->Success = false;
        errorData->ErrorMessage = "Unknown error occurred in native code";
        return errorData;
    }
}

bool WeatherInterop::ValidateReminderData(ManagedReminderData^ reminder) {
    if (reminder == nullptr) return false;

    try {
        // 简化验证逻辑，避免 C++ DLL 调用问题
        // 直接在 C++/CLI 层进行验证

        // 基本字段验证
        if (reminder->UserId <= 0) {
            return false;
        }

        if (String::IsNullOrEmpty(reminder->Location)) {
            return false;
        }

        if (String::IsNullOrEmpty(reminder->Date)) {
            return false;
        }

        if (String::IsNullOrEmpty(reminder->Time)) {
            return false;
        }

        // 所有基本验证通过
        return true;

        /* 原来的 C++ DLL 调用代码 - 暂时注释避免问题
        if (!InitializeNativeDll() || pValidateReminderData == nullptr) {
            return false;
        }

        ReminderData nativeReminder;
        memset(&nativeReminder, 0, sizeof(ReminderData));

        nativeReminder.id = reminder->Id;
        nativeReminder.userId = reminder->UserId;

        if (reminder->Time != nullptr) {
            marshal_context ctx;
            const char* timeStr = ctx.marshal_as<const char*>(reminder->Time);
            strcpy_s(nativeReminder.time, sizeof(nativeReminder.time), timeStr);
        }

        if (reminder->Location != nullptr) {
            marshal_context ctx;
            const char* locationStr = ctx.marshal_as<const char*>(reminder->Location);
            strcpy_s(nativeReminder.location, sizeof(nativeReminder.location), locationStr);
        }

        if (reminder->Date != nullptr) {
            marshal_context ctx;
            const char* dateStr = ctx.marshal_as<const char*>(reminder->Date);
            strcpy_s(nativeReminder.date, sizeof(nativeReminder.date), dateStr);
        }

        return pValidateReminderData(&nativeReminder) == 1;
        */
    }
    catch (...) {
        return false;
    }
}

int WeatherInterop::CalculateReminderHash(ManagedReminderData^ reminder) {
    if (reminder == nullptr) return 0;

    try {
        // 简化哈希计算，直接在 C++/CLI 层计算
        int hash = 0;
        hash = hash * 31 + reminder->UserId;

        if (!String::IsNullOrEmpty(reminder->Location)) {
            // 简单的字符串哈希
            for each (char c in reminder->Location) {
                hash = hash * 31 + c;
            }
        }

        if (!String::IsNullOrEmpty(reminder->Date)) {
            for each (char c in reminder->Date) {
                hash = hash * 31 + c;
            }
        }

        return hash;

        /* 原来的 C++ DLL 调用代码 - 暂时注释
        if (!InitializeNativeDll() || pCalculateReminderHash == nullptr) {
            return 0;
        }

        ReminderData nativeReminder;
        memset(&nativeReminder, 0, sizeof(ReminderData));

        nativeReminder.id = reminder->Id;
        nativeReminder.userId = reminder->UserId;

        if (reminder->Time != nullptr) {
            marshal_context ctx;
            const char* timeStr = ctx.marshal_as<const char*>(reminder->Time);
            strcpy_s(nativeReminder.time, sizeof(nativeReminder.time), timeStr);
        }

        if (reminder->Location != nullptr) {
            marshal_context ctx;
            const char* locationStr = ctx.marshal_as<const char*>(reminder->Location);
            strcpy_s(nativeReminder.location, sizeof(nativeReminder.location), locationStr);
        }

        if (reminder->Date != nullptr) {
            marshal_context ctx;
            const char* dateStr = ctx.marshal_as<const char*>(reminder->Date);
            strcpy_s(nativeReminder.date, sizeof(nativeReminder.date), dateStr);
        }

        return pCalculateReminderHash(&nativeReminder);
        */
    }
    catch (...) {
        return 0;
    }
}

String^ WeatherInterop::FormatWeatherDescription(String^ description) {
    if (description == nullptr) return nullptr;

    try {
        if (!InitializeNativeDll() || pFormatWeatherDescription == nullptr) {
            return description;
        }

        marshal_context ctx;
        const char* nativeDesc = ctx.marshal_as<const char*>(description);

        size_t len = strlen(nativeDesc);
        char* modifiableDesc = new char[len + 1];
        strcpy_s(modifiableDesc, len + 1, nativeDesc);

        pFormatWeatherDescription(modifiableDesc, static_cast<int>(len + 1));

        String^ result = gcnew String(modifiableDesc);
        delete[] modifiableDesc;

        return result;
    }
    catch (...) {
        return description;
    }
}

ManagedWeatherData^ WeatherInterop::ParseWeatherJson(String^ jsonResponse, String^ targetDate) {
    if (jsonResponse == nullptr || targetDate == nullptr) return nullptr;

    try {
        if (!InitializeNativeDll() || pParseWeatherJson == nullptr) {
            ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
            errorData->Success = false;
            errorData->ErrorMessage = "Failed to load native DLL or function";
            return errorData;
        }

        marshal_context ctx;
        const char* nativeJson = ctx.marshal_as<const char*>(jsonResponse);
        const char* nativeDate = ctx.marshal_as<const char*>(targetDate);

        WeatherData weatherData;
        memset(&weatherData, 0, sizeof(WeatherData));
        int result = pParseWeatherJson(nativeJson, &weatherData, nativeDate);

        ManagedWeatherData^ managedData = gcnew ManagedWeatherData();
        managedData->MaxTemperature = weatherData.maxTemperature;
        managedData->MinTemperature = weatherData.minTemperature;
        managedData->Description = gcnew String(weatherData.description);
        managedData->Wind = gcnew String(weatherData.wind);
        managedData->Date = gcnew String(weatherData.date);
        managedData->Location = gcnew String(weatherData.location);
        managedData->Success = (weatherData.success == 1);
        managedData->ErrorMessage = gcnew String(weatherData.errorMessage);

        return managedData;
    }
    catch (const std::exception& e) {
        ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
        errorData->Success = false;
        errorData->ErrorMessage = gcnew String(e.what());
        return errorData;
    }
    catch (...) {
        ManagedWeatherData^ errorData = gcnew ManagedWeatherData();
        errorData->Success = false;
        errorData->ErrorMessage = "Unknown error occurred during JSON parsing";
        return errorData;
    }
}