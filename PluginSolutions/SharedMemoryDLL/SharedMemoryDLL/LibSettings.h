#pragma once

#ifdef SHAREDMEMORYDLL_EXPORTS
#define LIB_API __declspec(dllexport)
#elif SHAREDMEMORYDLL_IMPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif