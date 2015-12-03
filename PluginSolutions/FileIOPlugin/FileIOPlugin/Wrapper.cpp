#include "Wrapper.h"
#include <fstream>
#include <string>

#pragma warning(disable: 4996)

void saveAscii(char* path, char* data, bool append)
{
	int flags = std::ios::out;

	if (append)
	{
		flags = flags | std::ios::app;
	}

	std::ofstream out(path, flags);
	std::string stringData(data);

	if (out.is_open())
	{
		out << stringData;
		out.close();
		out.clear();
	}
}

void saveBinary(char* path, char* data, bool append)
{
	int flags = std::ios::out | std::ios::binary;

	if (append)
	{
		flags = flags | std::ios::app;
	}

	std::ofstream out(path, flags);

	if (out.is_open())
	{
		out.write(data, strlen(data));
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

char* loadBinary(char* path)
{
	std::streampos begin, end;
	std::ifstream in(path, std::ios::binary);
	begin = in.tellg();
	in.seekg(0, std::ios::end);
	end = in.tellg();
	in.seekg(0, std::ios::beg);

	int length = end - begin;

	char* result = new char[length + 1];

	if (in.is_open())
	{
		in.read(result, length);

		in.close();
		in.clear();
	}

	result[length] = '\0';

	return result;
}