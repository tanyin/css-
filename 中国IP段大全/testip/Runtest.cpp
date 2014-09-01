#include <iostream>
using namespace std;
typedef struct node 
{
	__int64 startip;	
	__int64 endip;
};
node num[50000];

int main()
{
	int index = 1;
	FILE *fp=fopen("testip.txt","r");
	if (fp == NULL) return 0;
	while (scanf("%I64d%I64d",&num[index].startip,&num[index].endip)!=EOF) index++;
	char strip[100];
	__int64 ip_value,a,b,c,d;
	while (fscanf(fp,"%s",strip)!=EOF)
	{
		sscanf(strip,"%I64d.%I64d.%I64d.%I64d",&a,&b,&c,&d);
		//printf("test ip : %s\n",strip);
		ip_value = a*256*256*256+b*256*256+c*256+d;
		int i;
		i = 1;
		bool flag = false;
		while (i < index && flag == false)
		{
			if (num[i].startip == 0 && num[i].endip == 0) break;
			if (num[i].startip>ip_value) i = i * 2;
			else if (num[i].endip<ip_value) i = i * 2 + 1;
			else flag = true;
		}
		if (flag) printf("true\n");
		else printf("false %s\n",strip);
	}
	return 0;
}
