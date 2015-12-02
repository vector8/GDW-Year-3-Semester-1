/*
*	Source: Code adapted from https://msdn.microsoft.com/en-us/library/windows/desktop/ms686958(v=vs.85).aspx
*/

#include <windows.h> 
#include <memory.h> 
#include "SharedMemory.h"

static LPVOID lpvMem = NULL;      // pointer to shared memory
static HANDLE hMapObject = NULL;  // handle to file mapping

// The DLL entry-point function sets up shared memory using a 
// named file-mapping object. 
BOOL WINAPI DllMain(HINSTANCE hinstDLL,  // DLL module handle
	DWORD fdwReason,              // reason called 
	LPVOID lpvReserved)           // reserved 
{
	BOOL fInit, fIgnore;

	switch (fdwReason)
	{
		// DLL load due to process initialization or LoadLibrary

	case DLL_PROCESS_ATTACH:

		// Create a named file mapping object

		hMapObject = CreateFileMapping(
			INVALID_HANDLE_VALUE,   // use paging file
			NULL,                   // default security attributes
			PAGE_READWRITE,         // read/write access
			0,                      // size: high 32-bits
			SHMEMSIZE,              // size: low 32-bits
			TEXT("dllmemfilemap")); // name of map object
		if (hMapObject == NULL)
			return FALSE;

		// The first process to attach initializes memory

		fInit = (GetLastError() != ERROR_ALREADY_EXISTS);

		// Get a pointer to the file-mapped shared memory

		lpvMem = MapViewOfFile(
			hMapObject,     // object to map view of
			FILE_MAP_WRITE, // read/write access
			0,              // high offset:  map from
			0,              // low offset:   beginning
			0);             // default: map entire file
		if (lpvMem == NULL)
			return FALSE;

		// Initialize memory if this is the first process

		if (fInit)
			memset(lpvMem, '\0', SHMEMSIZE);

		break;

		// The attached process creates a new thread

	case DLL_THREAD_ATTACH:
		break;

		// The thread of the attached process terminates

	case DLL_THREAD_DETACH:
		break;

		// DLL unload due to process termination or FreeLibrary

	case DLL_PROCESS_DETACH:

		// Unmap shared memory from the process's address space

		fIgnore = UnmapViewOfFile(lpvMem);

		// Close the process's handle to the file-mapping object

		fIgnore = CloseHandle(hMapObject);

		break;

	default:
		break;
	}

	return TRUE;
	UNREFERENCED_PARAMETER(hinstDLL);
	UNREFERENCED_PARAMETER(lpvReserved);
}

// SetSharedMem sets the contents of the shared memory 
VOID __cdecl SetSharedMem(char* lpszBuf)
{
	char* lpszTmp;
	DWORD dwCount = 1;

	// Get the address of the shared memory block
	lpszTmp = (char*)lpvMem;

	// Copy the data into shared memory
	memcpy(lpszTmp, lpszBuf, SHMEMSIZE);
}

// GetSharedMem gets the contents of the shared memory
VOID __cdecl GetSharedMem(char* lpszBuf, DWORD cchSize)
{
	char* lpszTmp;

	// Get the address of the shared memory block
	lpszTmp = (char*)lpvMem;

	// Copy from shared memory into the caller's buffer
	memcpy(lpszBuf, lpszTmp, SHMEMSIZE);
}