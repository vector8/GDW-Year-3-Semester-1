#include "MouseInput.h"
#include <stdlib.h>

HWND targetWindowHandle = 0;
HHOOK mouseHook;
POINT currentMousePosition;
bool leftButtonDown = false, middleButtonDown = false, rightButtonDown = false;
bool leftButtonDownFirst = false, middleButtonDownFirst = false, rightButtonDownFirst = false;

// Forward declarations.
static LRESULT CALLBACK mouseCallback(int message, WPARAM wParam, LPARAM lParam);

bool setHookTarget(HWND wHnd)
{
	targetWindowHandle = wHnd;

	DWORD lProcessID = 0;

	GetWindowThreadProcessId(targetWindowHandle, &lProcessID);
	//std::cout << "ProcessID: " << lProcessID << std::endl;

	mouseHook = SetWindowsHookEx(WH_MOUSE_LL, (HOOKPROC)mouseCallback, NULL, 0);

	if (!mouseHook)
		return false;

	return true;
}

void shutdownMousePlugin()
{
	UnhookWindowsHookEx(mouseHook);
}

static LRESULT CALLBACK mouseCallback(int message, WPARAM wParam, LPARAM lParam)
{
	if (message < 0) return NULL;

	PMSLLHOOKSTRUCT lMEvent = (PMSLLHOOKSTRUCT)lParam;

	switch (wParam)
	{
	case WM_MOUSEMOVE:
		//std::cout << "Mouse: " << lMEvent->pt.x << " , " << lMEvent->pt.y << std::endl;
		break;
	case WM_LBUTTONDOWN:
		leftButtonDown = true;
		leftButtonDownFirst = true;
		//std::cout << "Left mouse button down" << std::endl;
		break;
	case WM_LBUTTONUP:
		leftButtonDown = false;
		leftButtonDownFirst = false;
		//std::cout << "Left mouse button up" << std::endl;
		break;
	case WM_MBUTTONDOWN:
		middleButtonDown = true;
		middleButtonDownFirst = true;
		//std::cout << "Middle mouse button down" << std::endl;
		break;
	case WM_MBUTTONUP:
		middleButtonDown = false;
		middleButtonDownFirst = false;
		//std::cout << "Middle mouse button up" << std::endl;
		break;
	case WM_RBUTTONDOWN:
		rightButtonDown = true;
		rightButtonDownFirst = true;
		//std::cout << "Right mouse button down" << std::endl;
		break;
	case WM_RBUTTONUP:
		rightButtonDown = false;
		rightButtonDownFirst = false;
		//std::cout << "Right mouse button up" << std::endl;
		break;
	default:
		break;
	}

	currentMousePosition = lMEvent->pt;
	ScreenToClient(targetWindowHandle, &currentMousePosition);

	return CallNextHookEx(NULL, message, wParam, lParam);
}

int getMouseX()
{
	return currentMousePosition.x;
}

int getMouseY()
{
	return currentMousePosition.y;
}

bool isMouseButtonDown(int buttonID)
{
	switch (buttonID)
	{
	case 0:
		return leftButtonDown;
	case 1:
		return middleButtonDown;
	case 2:
		return rightButtonDown;
	default:
		return false;
	}
}

bool isMouseButtonDownFirstTime(int buttonID)
{
	bool result = false;
	switch (buttonID)
	{
	case 0:
		result = leftButtonDownFirst;
		leftButtonDownFirst = false;
		break;
	case 1:
		result = middleButtonDownFirst;
		middleButtonDownFirst = false;
		break;
	case 2:
		result = rightButtonDownFirst;
		rightButtonDownFirst = false;
		break;
	default:
		break;
	}

	return result;
}