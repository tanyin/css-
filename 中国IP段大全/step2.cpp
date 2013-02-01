#include <iostream>
using namespace std;
__int64 ip[5]={0};
int main()
{
	int index = 0;
	__int64 ip_value;
	char str[100]={0};
	int j=0,i;
	while (scanf("%s",str)!=EOF)
	{
		sscanf(str,"%I64d.%I64d.%I64d.%I64d:%I64d",&ip[0],&ip[1],&ip[2],&ip[3],&ip[4]);
		ip_value = ip[0]*256*256*256+ip[1]*256*256+ip[2]*256+ip[3];
		printf("%I64d %I64d\n",ip_value,ip_value+ip[4]);
	}
	return 0;
}
