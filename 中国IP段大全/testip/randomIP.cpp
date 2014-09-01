#include <iostream>
#include <time.h>
using namespace std;
typedef struct node
{
	__int64 startip;
	__int64 endip;	
};

node num[50000];

int main()
{
	srand(time(0));
	int index = 0;;
	while (scanf("%I64d%I64d",&num[index].startip,&num[index].endip)!=EOF)	index++;
	__int64 a,b,c,d;
	__int64 ipvalue;
	for (int i=0;i<index;i++){
		ipvalue = rand()%(num[i].endip-num[i].startip)+num[i].startip;
		a = ipvalue % 256;
		b = ipvalue / 256 % 256;
	 	c = ipvalue / 256 / 256 % 256;
		d = ipvalue / 256 / 256 / 256 % 256;
		printf("%I64d.%I64d.%I64d.%I64d\n",d,c,b,a);
	}
	
	for (int i=0;i<10;i++)
	{
		printf("%d.%d.%d.%d\n",rand()%256,rand()%256,rand()%256,rand()%256);
	}
	return 0;
}
