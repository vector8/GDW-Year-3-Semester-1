#pragma once
#include "LibSettings.h"
#include <string>

enum class CommandType
{
	Add,
	Commit,
	Create,
	Error,
	Forget,
	Pull,
	Push,
	Remove,
	SetLocal,
	SetRemote,
	Update,
	NUMBER_COMMAND_TYPES
};

struct ParsedCommand
{
	CommandType type;
	std::string arg1;
};

class HgCommandManager
{
private:
	bool running = true;
	std::string localURL = "";
	std::string remoteURL = "";
	std::string errorMsg;
	bool errorStatus;

public:
	HgCommandManager();

	void runCommand(ParsedCommand command);
	bool hasChanged();

	void setError(std::string msg);
	void clearError();
	bool getErrorStatus();
	std::string getErrorMessage();
};