#include <iostream>
#include "LibSettings.h"
#include <WinSock2.h>

#pragma comment (lib, "ws2_32.lib")



class Network
{
private:
	SOCKET s;
	struct sockaddr_in sockAddr;
	int recv_len, slen = sizeof(sockAddr);
	char* server;
	char buf[512];
	char msg[512];

public:
	Network();

	void initialize();
	void update();
	char setServer();
	//void Send();
	//void Receive();

};