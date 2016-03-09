#pragma once

#include "LibSettings.h"

#ifdef __cplusplus
extern"C"
{
#endif

	LIB_API void setServer(char* SERVER);
	LIB_API void initializeClient();
	LIB_API void initializeServer();
	LIB_API void receive();
	LIB_API void sendTo(char* msg);


#ifdef __cplusplus
}
#endif