#pragma once
#include <windows.h> 

#define SHMEMSIZE 691200

VOID __cdecl SetSharedMem(char* lpszBuf);

VOID __cdecl GetSharedMem(char* lpszBuf, DWORD cchSize);