// pch.h - 预编译头文件
#pragma once

// Windows 相关头文件
#include <windows.h>
#include <winhttp.h>

// C++ 标准库
#include <string>
#include <iostream>
#include <exception>
#include <stdexcept>

// C++/CLI 相关头文件
#include <vcclr.h>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

// .NET Framework 命名空间
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

// 原生 DLL 头文件
#include "..\TravelMate.NativeDLL\WeatherNative.h"

//================================================
// pch.cpp - 预编译头文件实现
// 
// 文件内容：
// #include "pch.h"
// 
// 注意：这个文件只包含 #include "pch.h"，用于生成预编译头文件
//================================================