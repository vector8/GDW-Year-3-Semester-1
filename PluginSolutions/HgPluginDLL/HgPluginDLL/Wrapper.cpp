#include "Wrapper.h"

HgCommandManager instance;

void runCommand(int commandType, char* arg)
{
	instance.clearError();

	if (commandType < (int) CommandType::NUMBER_COMMAND_TYPES)
	{
		CommandType t = static_cast<CommandType>(commandType);

		ParsedCommand cmd;
		cmd.type = t;
		cmd.arg1 = std::string(arg);
		instance.runCommand(cmd);
	}
	else
	{
		instance.setError("Unrecognized command.");
	}
}

bool hasChanged()
{
	return instance.hasChanged();
}

bool getErrorStatus()
{
	return instance.getErrorStatus();
}

char* getErrorMessage()
{
	return (char*)instance.getErrorMessage().c_str();
}