#include "HgCommandManager.h"

#include <iostream>
#include <fstream>
#include <Windows.h>
#include "Utilities.h"

HgCommandManager::HgCommandManager()
{

}

bool HgCommandManager::hasChanged()
{
	std::string fileName = "tempStatusFile1891321.txt";
	system(("hg status >> " + fileName).c_str());
	std::ifstream file;
	file.open(fileName);
	std::string line;
	int count = 0;
	bool dirty = false;
	while (std::getline(file, line))
	{
		count++;

		if (count > 1)
		{
			dirty = true;
			break;
		}
	}
	file.close();
	std::remove(fileName.c_str());

	return dirty;
}

void HgCommandManager::runCommand(ParsedCommand command)
{
	std::string hgCommand = "";

	std::string local = localURL;

	if (local.length() > 0)
	{
		local = "-R " + local;
	}

	switch (command.type)
	{
	case CommandType::Add:
	{
		std::string arg = command.arg1;
		if (command.arg1.length() > 0)
		{
			arg = "\"" + arg + "\"";
		}
		hgCommand = "hg add " + arg + " " + local;
	}
		break;
	case CommandType::Commit:
	{
		hgCommand = "hg commit -m \"" + command.arg1 + "\"";
		if (localURL.length() > 0)
		{
			hgCommand += " --cwd " + localURL;
		}
	}
		break;
	case CommandType::Create:
	{
		hgCommand = "hg init";
		if (localURL.length() > 0)
		{
			hgCommand += " --cwd " + localURL;
		}
	}
		break;
	case CommandType::Forget:
	{
		hgCommand = "hg forget " + command.arg1 + " " + local;
	}
		break;
	case CommandType::Pull:
	{
		if (remoteURL.length() == 0)
		{
			setError("Remote URL not set. Use setremote command before pushing or pulling.");
			return;
		}
		else
		{
			hgCommand = "hg pull " + remoteURL + " " + local;
		}
	}
		break;
	case CommandType::Push:
	{
		if (remoteURL.length() == 0)
		{
			setError("Remote URL not set. Use setremote command before pushing or pulling.");
			return;
		}
		else
		{
			hgCommand = "hg push " + remoteURL + " " + local;
		}
	}
		break;
	case CommandType::Remove:
	{
		hgCommand = "hg remove " + command.arg1 + " " + local;
	}
		break;
	case CommandType::SetLocal:
	{
		localURL = "\"" + command.arg1 + "\"";
		return;
	}
		break;
	case CommandType::SetRemote:
	{
		remoteURL = command.arg1;
		return;
	}
		break;
	case CommandType::Update:
	{
		hgCommand = "hg update " + local;
	}
		break;
	default:
		return;
	}

	//std::string s = "echo " + hgCommand + " && pause && " + hgCommand + " && pause";
	system(hgCommand.c_str());
}

void HgCommandManager::setError(std::string msg)
{
	errorStatus = true;
	errorMsg = msg;
}

void HgCommandManager::clearError()
{
	errorStatus = false;
	errorMsg = "";
}

bool HgCommandManager::getErrorStatus()
{
	return errorStatus;
}

std::string HgCommandManager::getErrorMessage()
{
	return errorMsg;
}