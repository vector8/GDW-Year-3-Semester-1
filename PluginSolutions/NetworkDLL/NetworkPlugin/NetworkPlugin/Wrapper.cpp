#include "Wrapper.h"
#include "Network.h"

Network instance;

void setServer(char* SERVER)
{
	instance.setServer(SERVER);
}

int initializeClient()
{
	return instance.initializeClient();
}

int initializeServer()
{
	return instance.initializeServer();
}

int receive()
{
	return instance.receive();

}

int sendTo(char* msg)
{
	return instance.send(msg);
}

char* readBuffer()
{
	return instance.readBuffer();
}

void clearBuffer()
{
	instance.clearBuffer();
}

int getLength()
{
	return instance.getLength();
}

void closeConnections()
{
	instance.closeConnections();
}