#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include "LibSettings.h"
#include <WinSock2.h>

#pragma comment (lib, "ws2_32.lib")



class Network
{
private:
	SOCKET s;
	struct sockaddr_in sockAddr, server;
	int recv_len, slen = sizeof(sockAddr);
	char* serverIP;
	char buf[512];
	char msg[512];

public:
	Network();

	void initializeClient();
	void initializeServer();
	void setServer(char* SERVER);
	void send(char* msg);
	void receive();

};