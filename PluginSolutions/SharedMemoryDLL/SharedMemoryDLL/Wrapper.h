#include "LibSettings.h"

#ifdef __cplusplus    
extern "C"
{          
#endif

	LIB_API void setFrame(unsigned char* data);
	LIB_API unsigned char* getFrame();

#ifdef __cplusplus
}
#endif