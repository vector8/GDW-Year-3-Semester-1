#include "Network.h"

Network::Network()
{

}

void Network::setServer(char* SERVER)
{
	serverIP = SERVER;
}

int Network::initializeClient()
{
	//initiate the network
	WSADATA wsa;

	//initiate winsock
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		return WSAGetLastError();
	}
	//create socket0
	s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (s == -1)
	{
		return WSAGetLastError();
	}

	//set up address structure
	memset((char *)&sockAddr, 0, sizeof(sockAddr));
	sockAddr.sin_family = AF_INET;
	sockAddr.sin_port = htons(8888);
	sockAddr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);
	
	return 0;
}

int Network::initializeServer()
{
	//initiate the network
	WSADATA wsa;

	//initiate winsock
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		return WSAGetLastError();
	}
	//create socket
	s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (s == -1)
	{
		return WSAGetLastError();
	}
	//set up address structure
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(8888);
	
	//bind server
	if (bind(s, (struct sockaddr*)&server, sizeof(server)) == -1)
	{
		return WSAGetLastError();
	}

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);

	return 0;
}

int Network::send(char* msg)
{
	if (sendto(s, msg, strlen(msg), 0, (struct sockaddr *) &sockAddr, slen) == -1)
	{
		return WSAGetLastError();
	}
	return 0;
}

int Network::receive()
{
	if (recv_len = recvfrom(s, buf, 512, 0, (struct sockaddr*) &sockAddr, &slen) == -1)
	{
		return WSAGetLastError();
	}
	return 0;
}

char* Network::readBuffer()
{
	return buf;
}

void Network::clearBuffer()
{
	memset(buf, '\0', sizeof(char)* 512);
}

int Network::getLength()
{
	return recv_len;
}

void Network::closeConnections()
{
	shutdown(s, SD_SEND);
	closesocket(s);
	WSACleanup();
}