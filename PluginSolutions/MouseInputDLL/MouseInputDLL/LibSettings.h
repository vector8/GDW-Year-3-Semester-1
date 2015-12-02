#pragma once

#ifdef MOUSEINPUTDLL_EXPORTS
#define LIB_API __declspec(dllexport)
#elif MOUSEINPUTDLL_IMPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif