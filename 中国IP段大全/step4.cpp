#include <iostream>
using namespace std;
typedef struct node 
{
	__int64 startip;
	__int64 endip;
};
node num[50000];
int count = 0;
node num1[50000];
int flag[50000];
int maxnum = 0;
void gui(int l,int r,int index)
{
	if (l>r) return ;
	int mid = (l + r) / 2;
	//printf("l : %d   r : %d  mid : %d  index : %d\n",l,r,mid,index);
	//system("pause");
	if (index > maxnum) maxnum = index;
	num1[index] = num[mid];
	gui(l,mid-1,index*2);
	gui(mid+1,r,index*2+1);
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
	gui(0,index-1,1);
	for (int i=1;i<=maxnum;i++)
		printf("%I64d %I64d\n",num1[i].startip,num1[i].endip);
	return 0;
}
