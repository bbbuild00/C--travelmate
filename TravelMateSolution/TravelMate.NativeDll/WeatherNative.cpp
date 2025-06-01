#include "pch.h"
#include "WeatherNative.h"
#include <Windows.h>
#include <winhttp.h>
#include <string>
#include <iostream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <cctype>

#pragma comment(lib, "winhttp.lib")

// JSON 解析辅助函数
std::string findJsonValue(const std::string& json, const std::string& key) {
    std::string searchKey = "\"" + key + "\":";
    size_t keyPos = json.find(searchKey);
    if (keyPos == std::string::npos) return "";

    size_t valueStart = json.find("\"", keyPos + searchKey.length());
    if (valueStart == std::string::npos) return "";
    valueStart++;

    size_t valueEnd = json.find("\"", valueStart);
    if (valueEnd == std::string::npos) return "";

    return json.substr(valueStart, valueEnd - valueStart);
}

// 字符串转换函数
std::wstring stringToWstring(const std::string& str) {
    if (str.empty()) return std::wstring();
    int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
    std::wstring wstrTo(size_needed, 0);
    MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], size_needed);
    return wstrTo;
}

// 实际的HTTP请求函数
std::string makeHttpRequest(const std::wstring& url) {
    std::string response;
    HINTERNET hSession = NULL;
    HINTERNET hConnect = NULL;
    HINTERNET hRequest = NULL;

    try {
        // 初始化 WinHTTP
        hSession = WinHttpOpen(L"Weather Service/1.0",
            WINHTTP_ACCESS_TYPE_DEFAULT_PROXY,
            WINHTTP_NO_PROXY_NAME,
            WINHTTP_NO_PROXY_BYPASS, 0);

        if (!hSession) return "";

        // 解析 URL
        URL_COMPONENTS urlComp = { 0 };
        urlComp.dwStructSize = sizeof(urlComp);
        urlComp.dwSchemeLength = -1;
        urlComp.dwHostNameLength = -1;
        urlComp.dwUrlPathLength = -1;
        urlComp.dwExtraInfoLength = -1;

        if (!WinHttpCrackUrl(url.c_str(), (DWORD)url.length(), 0, &urlComp)) {
            return "";
        }

        std::wstring hostname(urlComp.lpszHostName, urlComp.dwHostNameLength);
        std::wstring path(urlComp.lpszUrlPath, urlComp.dwUrlPathLength);
        if (urlComp.lpszExtraInfo) {
            path += std::wstring(urlComp.lpszExtraInfo, urlComp.dwExtraInfoLength);
        }

        // 连接到服务器
        hConnect = WinHttpConnect(hSession, hostname.c_str(),
            urlComp.nPort, 0);

        if (!hConnect) return "";

        // 创建请求
        DWORD flags = (urlComp.nScheme == INTERNET_SCHEME_HTTPS) ? WINHTTP_FLAG_SECURE : 0;
        hRequest = WinHttpOpenRequest(hConnect, L"GET", path.c_str(),
            NULL, WINHTTP_NO_REFERER, WINHTTP_DEFAULT_ACCEPT_TYPES, flags);

        if (!hRequest) return "";

        // 发送请求
        if (!WinHttpSendRequest(hRequest, WINHTTP_NO_ADDITIONAL_HEADERS, 0,
            WINHTTP_NO_REQUEST_DATA, 0, 0, 0)) {
            return "";
        }

        // 接收响应
        if (!WinHttpReceiveResponse(hRequest, NULL)) return "";

        // 读取数据
        DWORD dwSize = 0;
        do {
            DWORD dwDownloaded = 0;
            if (!WinHttpQueryDataAvailable(hRequest, &dwSize)) break;

            if (dwSize == 0) break;

            char* pszOutBuffer = new char[dwSize + 1];
            ZeroMemory(pszOutBuffer, dwSize + 1);

            if (WinHttpReadData(hRequest, (LPVOID)pszOutBuffer, dwSize, &dwDownloaded)) {
                response.append(pszOutBuffer, dwDownloaded);
            }

            delete[] pszOutBuffer;
        } while (dwSize > 0);

    }
    catch (...) {
        // 处理异常
    }

    // 清理资源
    if (hRequest) WinHttpCloseHandle(hRequest);
    if (hConnect) WinHttpCloseHandle(hConnect);
    if (hSession) WinHttpCloseHandle(hSession);

    return response;
}

// 导出函数实现
extern "C" {

    WEATHERNATIVE_API int GetWeatherInfo(const char* location, const char* date, WeatherData* weatherData) {
        if (!location || !date || !weatherData) {
            return 0;
        }

        try {
            // 初始化结构体
            memset(weatherData, 0, sizeof(WeatherData));

            // 构建真实的API请求URL
            std::string apiKey = "d956654a9b90181094c0636f7888f326";
            std::string urlStr = "https://restapi.amap.com/v3/weather/weatherInfo?key=" + apiKey +
                "&city=" + std::string(location) + "&extensions=all&output=JSON";

            std::wstring url = stringToWstring(urlStr);

            // 发起HTTP请求
            std::string response = makeHttpRequest(url);
            if (response.empty()) {
                strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "HTTP request failed");
                weatherData->success = 0;
                return 0;
            }

            // 解析JSON响应
            return ParseWeatherJson(response.c_str(), weatherData, date);

        }
        catch (...) {
            strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "Unknown error occurred");
            weatherData->success = 0;
            return 0;
        }
    }

    WEATHERNATIVE_API int ValidateReminderData(const ReminderData* reminder) {
        if (!reminder) return 0;

        // 验证必要字段
        if (reminder->userId <= 0) return 0;
        if (strlen(reminder->location) == 0) return 0;
        if (strlen(reminder->date) == 0) return 0;
        if (strlen(reminder->time) == 0) return 0;

        return 1;
    }

    WEATHERNATIVE_API int CalculateReminderHash(const ReminderData* reminder) {
        if (!reminder) return 0;

        // 简单的哈希计算
        int hash = 0;
        hash = hash * 31 + reminder->userId;

        const char* str = reminder->location;
        while (*str) {
            hash = hash * 31 + *str++;
        }

        str = reminder->date;
        while (*str) {
            hash = hash * 31 + *str++;
        }

        return hash;
    }

    WEATHERNATIVE_API void FormatWeatherDescription(char* description, int maxLength) {
        if (!description || maxLength <= 0) return;

        // 简单的格式化处理
        for (int i = 0; i < maxLength && description[i]; i++) {
            if (description[i] == '_') {
                description[i] = ' ';
            }
        }
    }

    WEATHERNATIVE_API int ParseWeatherJson(const char* jsonResponse, WeatherData* weatherData, const char* targetDate) {
        if (!jsonResponse || !weatherData || !targetDate) {
            return 0;
        }

        try {
            std::string json(jsonResponse);

            // 检查状态
            if (findJsonValue(json, "status") != "1") {
                strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "API returned error status");
                weatherData->success = 0;
                return 0;
            }

            // 查找 forecasts 数组
            size_t forecastsPos = json.find("\"forecasts\":");
            if (forecastsPos == std::string::npos) {
                strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "No forecasts data found");
                weatherData->success = 0;
                return 0;
            }

            // 查找 casts 数组
            size_t castsPos = json.find("\"casts\":", forecastsPos);
            if (castsPos == std::string::npos) {
                strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "No casts data found");
                weatherData->success = 0;
                return 0;
            }

            // 简单的JSON解析来查找目标日期的数据
            size_t searchStart = castsPos;
            std::string targetDateStr(targetDate);

            while (true) {
                size_t datePos = json.find("\"date\":\"" + targetDateStr, searchStart);
                if (datePos == std::string::npos) break;

                // 查找这个日期对应的天气数据
                size_t blockStart = json.rfind("{", datePos);
                size_t blockEnd = json.find("}", datePos);

                if (blockStart != std::string::npos && blockEnd != std::string::npos) {
                    std::string weatherBlock = json.substr(blockStart, blockEnd - blockStart + 1);

                    // 解析天气数据
                    std::string dayTemp = findJsonValue(weatherBlock, "daytemp");
                    std::string nightTemp = findJsonValue(weatherBlock, "nighttemp");
                    std::string dayWeather = findJsonValue(weatherBlock, "dayweather");
                    std::string dayPower = findJsonValue(weatherBlock, "daypower");

                    if (!dayTemp.empty() && !nightTemp.empty()) {
                        weatherData->maxTemperature = std::stof(dayTemp);
                        weatherData->minTemperature = std::stof(nightTemp);
                        strcpy_s(weatherData->description, sizeof(weatherData->description), dayWeather.c_str());
                        strcpy_s(weatherData->wind, sizeof(weatherData->wind), dayPower.c_str());
                        strcpy_s(weatherData->date, sizeof(weatherData->date), targetDate);
                        strcpy_s(weatherData->location, sizeof(weatherData->location), "");
                        weatherData->success = 1;
                        return 1;
                    }
                }

                searchStart = datePos + 1;
            }

            strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "Weather data for specified date not found");
            weatherData->success = 0;
            return 0;

        }
        catch (...) {
            strcpy_s(weatherData->errorMessage, sizeof(weatherData->errorMessage), "JSON parsing error");
            weatherData->success = 0;
            return 0;
        }
    }

}