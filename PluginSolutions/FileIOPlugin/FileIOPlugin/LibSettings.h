#pragma once

#ifdef FILEIOPLUGIN_EXPORTS
#define LIB_API __declspec(dllexport)
#elif FILEIOPLUGIN_IMPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif