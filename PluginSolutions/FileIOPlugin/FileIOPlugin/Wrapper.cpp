#include "Wrapper.h"
#include <fstream>
#include <string>

#pragma warning(disable: 4996)

void saveAscii(char* path, char* data)
{
	std::ofstream out(path);
	std::string stringData(data);

	if (out.is_open())
	{
		out << stringData;
		out.close();
		out.clear();
	}
}

void saveBinary(char* path, char* data, int length)
{
	std::ofstream out(path, std::ios::binary);

	if (out.is_open())
	{
		out.write(data, length);
		out.close();
		out.clear();
	}
}

char* loadAscii(char* path)
{
	std::string result;

	std::ifstream in(path);

	if (in.is_open())
	{
		std::string line;
		while (std::getline(in, line))
		{
			result += line + "\n";
		}

		in.close();
		in.clear();
	}

	char* cResult = new char[result.length()];
	strcpy(cResult, result.c_str());

	return cResult;
}

char* loadBinary(char* path, int length)
{
	char* result = new char[length];
	std::ifstream in(path, std::ios::binary);

	if (in.is_open())
	{
		in.read(result, length);

		in.close();
		in.clear();
	}

	return result;
}