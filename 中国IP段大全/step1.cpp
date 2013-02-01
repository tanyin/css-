#include <iostream>
#define part1 0
#define part2 2
using namespace std;
int main()
{
	char str[1000];
	
	int index = 0;
	while (scanf("%s",str)!=EOF){
		if (index % 2==part1) printf("%s\n",str);
		index++;
	}
	return 0;
}
