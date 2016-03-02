#include "Wrapper.h"
#include "Network.h"

Network instance;

void setServer(char* SERVER)
{
	instance.setServer(SERVER);
}

void initializeClient()
{
	instance.initializeClient();
}

void initializeServer()
{
	instance.initializeServer();
}

void receive()
{
	instance.receive();

}

void sendTo(char* msg)
{
	instance.send(msg);
}