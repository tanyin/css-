#include <iostream>
#include <algorithm>
using namespace std;
typedef struct node 
{
	__int64 startip;
	__int64 endip;
};
bool cmp(node a,node b)
{
	if (a.endip<b.startip) return true;
	return false;
}
node num[50000];

bool canEat(node a,node b)
{
	if (b.startip>=a.startip && b.startip<=a.endip) return true;
	if (b.endip<=a.endip && b.endip>=a.startip) return true;
	return false;
}
int main()
{
	__int64 a,b;
	int index = 0;
	while (scanf("%I64d %I64d",&a,&b)!=EOF){
		num[index].startip = a;
		num[index].endip = b;
		index++;
	}
	sort(num,num+index,cmp);
	int j = 1;
	for (int i=0;i<index;i+=j)
	{
		j = 1;
		a = num[i].startip;
		b = num[i].endip;
		while (i+j<index && canEat(num[i],num[i+j]))
		{
			num[i].startip = num[i].startip<num[i+j].startip?num[i].startip:num[i+j].startip;
			num[i].endip = num[i].endip>num[i+j].endip?num[i].endip:num[i+j].endip;			
			j++;
		}
		printf("%I64d %I64d\n",num[i].startip,num[i].endip);
	}
	return 0;
}
