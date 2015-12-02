#include "LibSettings.h"

#ifdef __cplusplus    
extern "C"
{
#endif

	LIB_API void saveAscii(char* path, char* data);
	LIB_API void saveBinary(char* path, char* data, int length);
	LIB_API char* loadAscii(char* path);
	LIB_API char* loadBinary(char* path, int length);

#ifdef __cplusplus
}
#endif