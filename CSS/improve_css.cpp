#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BEGIN_DESCIRP 0
#define END_DESCIRP 1
#define NO_DESCIRP 2

#define NO_BRACKET 3
#define LEFT_BRACKET 4
#define RIGHT_BRACKET 5
struct KEY_STR
{
    int line_number;
    char content[200];
    KEY_STR *pNextKey;
};
struct VALUE_STR
{
    int id;
    char* value;
    KEY_STR* pKey;
    VALUE_STR *pNextValue;
    VALUE_STR *pPreValue;
};
void str_trim(char *str);
void insert_key_to_value(VALUE_STR* pValue,KEY_STR* pKey);

VALUE_STR *pHead = NULL;

void init()
{
     if(pHead != NULL)
     {
        //销毁整个链表 
     }
     pHead = (VALUE_STR*)malloc(sizeof(VALUE_STR));
     pHead->value = NULL;
     pHead->pKey = NULL;
     pHead->pNextValue = NULL;
}
bool insert_before_value(VALUE_STR* pValue, VALUE_STR* pNewValue)
{
    (pValue->pPreValue)->pNextValue = pNewValue;
    pNewValue->pPreValue =  (pValue->pPreValue); 
    pNewValue->pNextValue = pValue;
    pValue->pPreValue = pNewValue;
    
}
bool content_in_value(VALUE_STR* pServerValue,char *content)
{
    KEY_STR* pTempKey = pServerValue->pKey;
    while(pTempKey != NULL)
    {
        if(strcmp(pTempKey->content,content) == 0)
        {
            return true;
        }
        pTempKey = pTempKey->pNextKey;
    }
    return false;
}
void combine_same_value(VALUE_STR* pServerValue,VALUE_STR *pClientValue)
{
    KEY_STR* pKeyTemp = pClientValue->pKey;
    while(pKeyTemp != NULL)
    {
        if(content_in_value(pServerValue,pKeyTemp->content))
        {
        }
        else
        {
             KEY_STR* pKey= (KEY_STR*)malloc(sizeof(KEY_STR));
             strcpy(pKey->content,pKeyTemp->content);
             pKey->line_number = pKeyTemp->line_number;
             pKey->pNextKey = NULL;
             insert_key_to_value(pServerValue,pKey); 
        }
        pKeyTemp = pKeyTemp->pNextKey;
    }
}
void insert_value(VALUE_STR* pValue)
{
      if(pHead->pNextValue == NULL)
      {
          pHead->pNextValue = pValue;
          pValue->pPreValue = pHead; 
          return;
      }
      VALUE_STR* pTemp = pHead->pNextValue;
      while(pTemp != NULL)
      {
          //重复的value ,不插入 
          if(strcmp(pValue->value,pTemp->value) == 0)
          {
              combine_same_value(pTemp,pValue);
              printf("重复:%s\n",pValue->value);
              return;
          }
          //升序排列，找到第一个比插入的值大的 
          else if(strcmp(pValue->value,pTemp->value) < 0)
          {
               insert_before_value(pTemp,pValue);
               return;
          }
          if(pTemp->pNextValue == NULL)
          {
              pTemp->pNextValue = pValue;
              pValue->pPreValue = pTemp;
              return;
          }
          pTemp = pTemp->pNextValue;
      }
}

void str_trim(char *str)
{
     if(str == NULL)
     {
            return;
     }
     char *temp_str = (char*)malloc((strlen(str)+1)*sizeof(char));
     memset(temp_str,0,(strlen(str)+1)*sizeof(char));
     if(temp_str == NULL)
     {
            return;
     }
     int i=0,j=strlen(str)-1;
     while(i<strlen(str) && (str[i]==' ' || str[i] == '\n' || str[i] == '\t'))
     {
         i++;
     }
     while(j>=0 && (str[j]==' ' || str[j] == '\n' || str[j] == '\t'))
     {
         j--;
     }
     int k;
     for(k=i;k<=j;k++)
     {
          temp_str[k-i] = str[k];
          if(k==j)
          {
              temp_str[k-i+1] = '\0';
          } 
     }
     memset(str,0,strlen(str)*sizeof(char));
     strcpy(str,temp_str);
}
int is_desciption(char *str)
{
    if(str == NULL)
    {
     return NO_DESCIRP;
    }
    if(strlen(str)<2)
    {
     return NO_DESCIRP;
    }
    if(str[strlen(str)-2] == '*' && str[strlen(str)-1] == '/')
    {
        return END_DESCIRP;
    }
    else if(str[0] == '/' && str[1] == '*')
    {
       return BEGIN_DESCIRP;
    }
    return NO_DESCIRP;
}

int deal_bracket(char *str)
{
    if(str == NULL)
    {
     return NO_BRACKET;
    }
    if(strlen(str) == 0)
    {
     return NO_BRACKET;
    }
    if(str[strlen(str)-1] == '{')
    {
        return LEFT_BRACKET;
    }
    if(str[strlen(str)-1] == '}')
    {
      return RIGHT_BRACKET;
    }
    return NO_BRACKET;
}
void strcatN(char* des, char* src, int length)
{
      if(des == NULL || src == NULL)
      {
          return;
      }
      int des_length = strlen(des);
      int src_length = strlen(src);
      if(length < 0)
      {
          length = 0;          
      }
      if(length > src_length)
      {
          length = src_length;
      }
      for(int i=0;i<length;i++)
      {
          des[des_length+i] = src[i];
      }
      des[des_length+length] = '\0';
} 
void insert_key_to_value(VALUE_STR* pValue,KEY_STR* pKey)
{
     if(pValue == NULL || pKey == NULL)
         return ;
     if(pValue->pKey == NULL)
     {
         pValue->pKey = pKey;
         return;
     }
     KEY_STR* pTempKey = pValue->pKey;
     if(strcmp(pTempKey->content,pKey->content) == 0)
     {
         //key重复,不插入，否则插入到最后面 
         return; 
     }
     else
     {
         while(pTempKey != NULL)
         {
             if(pTempKey->pNextKey == NULL)
             {
                 pTempKey->pNextKey = pKey;
                 break;
             }
             pTempKey = pTempKey->pNextKey;
         }
     }
}
VALUE_STR* new_value(char* keys,char* value,int id,int line_number)
{
    VALUE_STR* pNewValue = (VALUE_STR*)malloc(sizeof(VALUE_STR));
    pNewValue->value = (char*)malloc((strlen(value)+1)*sizeof(char));
    pNewValue->id = id;
    strcpy(pNewValue->value, value);
    pNewValue->pNextValue = NULL;
    pNewValue->pKey = NULL;
    char str_key[200];
    int count = 0;
    int keys_len = strlen(keys);
    memset(str_key,0,200);
    for(int i=0;i<=keys_len;i++)
    {
        if(i==(keys_len) || keys[i] == ',')
        {
             KEY_STR* pKey= (KEY_STR*)malloc(sizeof(KEY_STR));
             strcpy(pKey->content,str_key);
             pKey->line_number = line_number;
             pKey->pNextKey = NULL;
             memset(str_key,0,200);
             insert_key_to_value(pNewValue, pKey);
             count = 0;
             continue;
        }
        str_key[count++] = keys[i];
    }
    return pNewValue;
}
void deal_keys_value(char* keys,char* value,int count,int line_number)
{
    VALUE_STR* pNewValue = new_value(keys, value,count, line_number);
    insert_value(pNewValue);
}
void virtual_exchange(VALUE_STR* pValue1, VALUE_STR* pValue2)
{
      char *value = NULL;
      int Tempid = 0;
      KEY_STR* pTempKey = NULL;
      
      value = pValue1->value;
      pValue1->value = pValue2->value;
      pValue2->value = value;
      
      Tempid = pValue1->id;
      pValue1->id = pValue2->id;
      pValue2->id = Tempid;
      
      pTempKey = pValue1->pKey;
      pValue1->pKey =  pValue2->pKey;
      pValue2->pKey = pTempKey;
}
void sort_value() 
{
    if(pHead == NULL)
    {
        return;
    }
    VALUE_STR* pTempValueI = pHead->pNextValue; 
    VALUE_STR* pTempValueJ = NULL; 
    VALUE_STR* exchangeValue = NULL;
    int idI = 0;
    int minIDJ = 0xFFFFFFFF;
    while(pTempValueI!= NULL)
    {
            idI = pTempValueI->id;
            minIDJ = 100000000;
            exchangeValue = NULL;
            pTempValueJ =  pTempValueI->pNextValue;
            while(pTempValueJ != NULL)
            {
                if(pTempValueJ->id < minIDJ)
                {
                    minIDJ = pTempValueJ->id;
                    exchangeValue = pTempValueJ;
                }
                pTempValueJ = pTempValueJ->pNextValue;
            }
            if(exchangeValue != NULL && minIDJ < idI)
            {
                virtual_exchange(pTempValueI, exchangeValue);
            }
            pTempValueI = pTempValueI->pNextValue;
    }
}
void implode_keys(VALUE_STR* pValue, char *str)
{
    KEY_STR* pTempKey = pValue->pKey;
    while(pTempKey != NULL)
    {
         strcat(str,pTempKey->content);
         if(pTempKey->pNextKey != NULL)
         {
             strcat(str,", ");
         }
         pTempKey = pTempKey->pNextKey;
     }
}
void implode_value(char *value, char *str)
{
   int count = 0;
   str[count++] = ' ';
   str[count++] = ' ';
   for(int i=0; i<strlen(value); i++)
   {
       str[count++] = value[i];
       if(value[i] == ';')
       {
           str[count++] = '\n';
           if(i != strlen(value)-1)
           {
                str[count++] = ' ';
                str[count++] = ' ';
           }
       }
   }
   str[count++] = '\0';
}

void print_css()
{
     FILE *fo = fopen("newCSS.css","w");
     VALUE_STR* pTemp = pHead->pNextValue;
     char keys[10000];
     char value[10000];
     memset(keys,0,10000);
     memset(value,0,10000);
     while(pTemp != NULL)
     {
         implode_keys(pTemp,keys);
         implode_value(pTemp->value,value);
         fputs(keys,fo);
         fputs("{\n",fo);
         fputs(value,fo);
         fputs("}\n",fo);
         pTemp = pTemp->pNextValue;
         memset(keys,0,10000);
         memset(value,0,10000);
     }
     fclose(fo);
}

int main()
{
    init();
    FILE *fp = fopen("home.css","r");
    if(fp == NULL)
    {
          printf("%s","打开文件失败");
          return 1;
    }
    char buf[10000];
    char value[10000];
    char keys[100000];
    int line_number = 0;
    int desciption_type = END_DESCIRP;
    bool desciption_begin = false;
    int bracket_type = END_DESCIRP;
    int latst_bracket_type = RIGHT_BRACKET;
    int count = 0;
    while(fgets(buf,sizeof(buf),fp)!=NULL)
    {
          line_number++;
          bool has_key = false;
          bool has_value = false;
          str_trim(buf);
          if(strlen(buf) ==0)
          {
              continue;
          }
          desciption_type = is_desciption(buf);
          if(desciption_type ==  BEGIN_DESCIRP)
          {
              desciption_begin = true;
              continue;
          }
          else if(desciption_type == END_DESCIRP )
          {
              desciption_begin = false;
              continue;
          }
          else
          {
              if(desciption_begin)
              {
                  continue;
              }
          }
          bracket_type = deal_bracket(buf);
          if(bracket_type == LEFT_BRACKET)
          {
              if(strlen(buf) <= 1)
              {
                  latst_bracket_type = LEFT_BRACKET;
                  continue;
              }
              else
              {
                  strcatN(keys, buf, strlen(buf)-1);
              }
          }
          else if(bracket_type == RIGHT_BRACKET)
          {
              count++;
              deal_keys_value(keys, value,count, line_number);
              memset(keys,0,10000);
              memset(value,0,10000);
          }
          else
          {
              if(latst_bracket_type == RIGHT_BRACKET)
              {
                 strcatN(keys, buf, strlen(buf)); 
              }
              else if(latst_bracket_type == LEFT_BRACKET)
              {
                  strcatN(value, buf, strlen(buf));  
              }
          }
          if(bracket_type != NO_BRACKET)
          {
              latst_bracket_type = bracket_type;
          }
    }
    sort_value();
    print_css();
    printf("_______________%s_______________","处理完毕");
    system("pause");
    return 0;
} 


