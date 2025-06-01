#pragma once

#ifdef TRAVELMATENATIVEDLL_EXPORTS
#define WEATHERNATIVE_API __declspec(dllexport)
#else
#define WEATHERNATIVE_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

    // 天气数据结构
    typedef struct {
        float maxTemperature;
        float minTemperature;
        char description[256];
        char wind[256];
        char date[32];
        char location[256];
        int success; // 1: 成功, 0: 失败
        char errorMessage[512];
    } WeatherData;

    // 提醒数据结构
    typedef struct {
        int id;
        char time[32];
        char location[256];
        int userId;
        char date[32];
    } ReminderData;

    // 导出函数声明
    WEATHERNATIVE_API int GetWeatherInfo(const char* location, const char* date, WeatherData* weatherData);
    WEATHERNATIVE_API int ValidateReminderData(const ReminderData* reminder);
    WEATHERNATIVE_API int CalculateReminderHash(const ReminderData* reminder);
    WEATHERNATIVE_API void FormatWeatherDescription(char* description, int maxLength);
    WEATHERNATIVE_API int ParseWeatherJson(const char* jsonResponse, WeatherData* weatherData, const char* targetDate);

#ifdef __cplusplus
}
#endif