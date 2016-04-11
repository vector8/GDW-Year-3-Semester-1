#pragma once 

#ifdef NETWORKDLL_EXPORTS
#define LIB_API __declspec(dllexport)
#elif NETWORKDLL_IMPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif