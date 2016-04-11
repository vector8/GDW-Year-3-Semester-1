#pragma once

#include "LibSettings.h"
#include "string"

#ifdef __cplusplus
extern"C"
{
#endif

	LIB_API void setServer(char* SERVER);
	LIB_API int initializeClient();
	LIB_API int initializeServer();
	LIB_API int receive();
	LIB_API int sendTo(char* msg);
	LIB_API char* readBuffer();
	LIB_API int getLength();
	LIB_API void clearBuffer();
	LIB_API void closeConnections();

#ifdef __cplusplus
}
#endif