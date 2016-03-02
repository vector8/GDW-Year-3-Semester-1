#include "Network.h"

Network::Network()
{

}

void Network::setServer(char* SERVER)
{
	serverIP = SERVER;
}

void Network::initializeClient()
{
	//initiate the network
	WSADATA wsa;

	//initiate winsock
	WSAStartup(MAKEWORD(2, 2), &wsa);
	//create socket
	s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	//set up address structure
	memset((char *)&sockAddr, 0, sizeof(sockAddr));
	sockAddr.sin_family = AF_INET;
	sockAddr.sin_port = htons(8888);
	sockAddr.sin_addr.S_un.S_addr = inet_addr(serverIP);

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);
}

void Network::initializeServer()
{
	//initiate the network
	WSADATA wsa;

	//initiate winsock
	WSAStartup(MAKEWORD(2, 2), &wsa);
	//create socket
	s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	//set up address structure
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(8888);
	
	//bind server
	bind(s, (struct sockaddr*) &server, sizeof(server));

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);
}

void Network::send(char* msg)
{
	sendto(s, buf, strlen(msg), 0, (struct sockaddr *) &sockAddr, slen);
}

void Network::receive()
{
	recv_len = recv(s, buf, 512, 0);

	
}