#include "Network.h"

Network::Network()
{

}

char Network::setServer()
{
	//server = SERVER;
}

void Network::initialize()
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
	sockAddr.sin_addr.S_un.S_addr = inet_addr(server);

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);
}

void Network::update()
{
	//send packets
	sprintf_s(msg, sizeof(msg), "%f", msg);
	sendto(s, buf, strlen(msg), 0, (struct sockaddr *) &sockAddr, slen);

	//receive packets
	recv_len = recv(s, buf, 512, 0);
	
}