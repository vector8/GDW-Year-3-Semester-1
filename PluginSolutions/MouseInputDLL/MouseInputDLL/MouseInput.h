#pragma once

#include "LibSettings.h"
#include <Windows.h>

#ifdef __cplusplus
extern "C"
{
#endif

LIB_API bool setHookTarget(HWND wHnd);

LIB_API void shutdownMousePlugin();

LIB_API int getMouseX();

LIB_API int getMouseY();

LIB_API bool isMouseButtonDown(int buttonID);

LIB_API bool isMouseButtonDownFirstTime(int buttonID);

#ifdef __cplusplus
}
#endif