#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include "LibSettings.h"
#include "string"
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
	std::string* message;

public:
	Network();

	int initializeClient();
	int initializeServer();
	void setServer(char* SERVER);
	int send(char* msg);
	int receive();
	char* readBuffer();
	int getLength();
	void clearBuffer();

};