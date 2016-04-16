//Note:  This was in progress
//       Was never finished

#pragma comment(lib, "Ws2_32.lib")
#include <iostream>
#include "string"
#include <WinSock2.h>
#include <vector>

const int BUFFERSIZE = 512;

SOCKET s;
struct sockaddr_in sockAddr, server;
int recv_len, slen = sizeof(sockAddr);
char* serverIP;
char buf[BUFFERSIZE];
char msg[BUFFERSIZE];
std::vector<info> randQueue;
std::vector<info> hostQueue;

//std::vector<std::string> randQueue;
//std::vector<std::string> hostQueue;

struct info
{
	std::string message;
	SOCKET socket;
};

std::string getHostList()
{
	std::string list = "";
	for (int i = 0; i < hostQueue.size(); i++)
	{
		list += hostQueue[i].message.substr(2, 15) + ",";
	}
	list = list.substr(0, list.length() - 2);
	return list;
}

int main()
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
	
	while (true)
	{
		int error;
		if (recv_len = recvfrom(s, buf, BUFFERSIZE, 0, (struct sockaddr*) &sockAddr, &slen) == -1)
		{
			error = WSAGetLastError();
			if (buf != "")
			{
				std::string bufstring = buf;

				std::string option = bufstring.substr(0, 2);

				info receivedInfo;
				receivedInfo.message = bufstring;
				receivedInfo.socket = s;

				if (option == "QR")
				{
					randQueue.push_back(receivedInfo);
					//sprintf_s(msg, sizeof(msg), "%s", "SC");
					//sendto(s, msg, BUFFERSIZE, 0, (struct sockaddr*) &sockAddr, slen);
				}
				else if (option == "QH")
				{
					//check if hostname is in use
					hostQueue.push_back(receivedInfo);
					sprintf_s(msg, sizeof(msg), "%s", "SC");
					sendto(s, msg, BUFFERSIZE, 0, (struct sockaddr*) &sockAddr, slen);
				}
				else if (option == "RH")
				{
					sprintf_s(msg, sizeof(msg), "HL%s", getHostList());
					sendto(s, msg, BUFFERSIZE, 0, (struct sockaddr*) &sockAddr, slen);
					//response
				}
				else if (option == "QD")
				{
					//add code here
				}
			}
			else
			{
				Sleep(50);
			}
		}
	}

	return 0;
}