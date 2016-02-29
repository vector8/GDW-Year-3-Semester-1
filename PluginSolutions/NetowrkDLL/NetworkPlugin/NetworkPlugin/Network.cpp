#include <iostream>
#include <WinSock2.h>

#pragma comment (lib, "ws2_32.lib")

#define SERVER = "localhost"

//Socket Variables//
SOCKET s;
struct sockaddr_in sockAddr;
int slen = sizeof(sockAddr);
char buf[512];
char msg[512];
/////

void initialize()
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
	sockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER);

	//set blocking / non blocking (1 = non blocking)
	u_long iMode = 1;
	ioctlsocket(s, FIONBIO, &iMode);
}

int main()
{

}