#include "Utilities.h"

std::string toLower(const std::string &s)
{
	std::string result = s;

	for (std::string::iterator it = result.begin(); it != result.end(); it++)
	{
		*it = tolower(*it);
	}

	return result;
}