#include <iostream>
#include "LibSettings.h"
#include <WinSock2.h>

#pragma comment (lib, "ws2_32.lib")

#define SERVER = "localhost"

class Network
{
private:
	SOCKET s;
	struct sockaddr_in sockAddr;
	int slen = sizeof(sockAddr);
	char server;
	char buf[512];
	char msg[512];

public:
	void initialize();
	void update();
	char setServer();

};