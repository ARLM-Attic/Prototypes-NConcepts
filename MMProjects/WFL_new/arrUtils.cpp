
inline bool isOdd(int number)
{
    return(number%2);
}    

inline bool IsInArr(const int find, const int* arr, const int size)
{

    for(int i = 0; i < size; ++i)
    {
        if(arr[i] == find)
        {
            return(true);
        }
    }

    return(false);
}

inline bool copyInArr(int* dest, const int* src, const int size)
{
    for(int i = 0; i < size; i++)
    {
        dest[i] = src[i];
    }
    return(true);
}

inline int maxInArr(const int* arr, const int size, int &outIndex)
{
    int max = 0;
    int val = 0;
    int index = 0;

    for (int i = 0; i < size; i++)
    {
        val = arr[i];

        if (val > max)
        {
           max = val;
           index = i;       
        }
    }

    outIndex = index;
    return (max);
}