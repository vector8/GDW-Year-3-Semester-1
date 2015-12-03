#pragma once

#ifdef HGPLUGINDLL_EXPORTS
#define LIB_API __declspec(dllexport)
#elif HGPLUGINDLL_IMPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif