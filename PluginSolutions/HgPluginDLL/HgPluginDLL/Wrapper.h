#pragma once

#include "LibSettings.h"
#include "HgCommandManager.h"

#ifdef __cplusplus
extern "C"
{
#endif

	LIB_API void runCommand(int commandType, char* arg);
	LIB_API bool hasChanged();
	LIB_API bool getErrorStatus();
	LIB_API char* getErrorMessage();

#ifdef __cplusplus
}
#endif