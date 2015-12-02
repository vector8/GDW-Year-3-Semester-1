#include "Wrapper.h"
#include "SharedMemory.h"

void setFrame(unsigned char* d)
{
	SetSharedMem(reinterpret_cast<char*>(d));
}

unsigned char* getFrame()
{
	char tmp[SHMEMSIZE];
	unsigned char* out;

	GetSharedMem(tmp, SHMEMSIZE);

	out = reinterpret_cast<unsigned char*>(tmp);

	return out;
}