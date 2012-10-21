// WFL.cpp : main project file.

#include "stdafx.h"
#include <stdlib.h>
#include <stdio.h>
#include <windows.h>
#include <math.h>

const int MIN_WFL_VALUE     =   1;
const int MAX_WFL_VALUE     =   42;
const int PIK_WFL_VALUE     =   6;
const int MAX_NUM_OF_ODDS   =   3;

// If you want to use custom seed, enter it here (0 means no)
const int USE_SEED = 0;

//IMPORTANT CONFIGURATION OPTIONS!
const int NUM_OF_FATHER_VALUES_TO_USE = 3;
const int NUM_OF_GRANDFATHER_VALUES_TO_USE = 3;
const int NUM_OF_LEAST_VALUES_TO_USE = 1;
const int NUM_OF_SECOND_MEAN_VALUES_TO_USE = 4;
const int NUM_OF_WARM_VALUES_TO_USE = 2;
const int NUM_OF_HOT_VALUES_TO_USE = 1;

//IMPORTANT for how to generate Seed
bool bUseFatherInSeedGeneration = true;         // Global Flag indicating whether to use or not to use Father when calculation the seed

const int CALCULATION_BUFFER = 200;             // this buffer contains all the numbers calculated and used in subsequent
                                                // ticket generation ~200 should be enough ~allows me to not dynamically allocate storage
int SIZE_CALC = 0; // keeps track of size of calculation buffer
// Indexes into the Calculation Buffer
int INDEX_FirstMean = 0;
int SIZE_FirstMean = 0;
int INDEX_SecondMean = 0;
int SIZE_SecondMean = 0;
int INDEX_Father = 0;
int SIZE_Father = 0;
int INDEX_GrandFather = 0;
int SIZE_GrandFather = 0;
int INDEX_LEAST = 0;
int SIZE_LEAST = 0;
int INDEX_Warm = 0;
int SIZE_Warm = 0;
int INDEX_Hot = 0;
int SIZE_Hot = 0;

bool bAnalysisMode = false;                     // Sets the Analysis Mode
bool bGeneratingRandomNumberForFirstTime = true;
bool bNumbersPlayedAlreadyPrinted = false;      // For Analysis Mode
bool bValuesAlreadyCalculated = false;          // Again, to avoid repetition for analysis
int arrValues[MAX_WFL_VALUE] = {0};             // Holds the number of times each number has been played! - Don't modify
int arrWinningTicket[MAX_WFL_VALUE] = {0};      // Holds the numbers for the Winning Ticket (that are automatically generated)
int arrCalculated[CALCULATION_BUFFER] = {0};    // Buffer to hold all calculated number to be randomly inserted into the winning ticket
float fStdDeviation = 0;
unsigned int iStdDeviation = 0;                 // i holds the ceiling of fStdDeviation(float value rounded up)
unsigned int iPlayedAverage = 0;
unsigned int iFatheredAverage = 0;
unsigned int iGrandfatheredAverage = 0;
unsigned int Seed = 0;
unsigned int Milliseconds = 0;                   // just for my information
unsigned int iFatherChecksum = 0;                // for seed generation (uses checksum of previous lottery winner)

inline bool isOdd(int number)
{
    return(number%2);
}    

bool IsInArr(int find, int* arr, int arrSize)
{

    for(int i = 0; i < arrSize; ++i)
    {
        if(arr[i] == find)
        {
            return(true);
        }
    }

    return(false);
}

int MaxInArr(int* arr, int size, int &outIndex)
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

bool copyInArr(int* dest, int* src, int size)
{
    for(int i = 0; i < size; i++)
    {
        dest[i] = src[i];
    }
    return(true);
}

void WriteOut(int* arr, int arrSize, int _Seed)
{
    char szBuffer[200];
    sprintf(szBuffer, "WFL Ticket %02d.txt",_Seed);            
    int TicketNumber = 1;

    FILE *h = fopen(szBuffer, "w");

    fprintf(h, "\n       WFL Winning Ticket %02d     \n", _Seed);

    for(int i = 0; i < arrSize; ++i)
    {   
        // Print the Ticket Header:
        if((i == 0) || ((i % PIK_WFL_VALUE) == 0))
        {
            fprintf(h,"\n Ticket%01d: ",TicketNumber);
            ++TicketNumber;
        }

        fprintf(h," %02d ",arr[i]);        

    }

    fclose(h);        
}

void stripeNumbers(char* src, char* dest)
{

    int len = strlen(src);
    bool bNow = false;
    int j = 0; //destination counter

    for (int i = 0; i < len; i++)
    {
        if (src[i] == ',')
        {
          bNow = true;

          // also skip the " character after the ','
          ++i;

          continue;
        }        

        if (bNow)
        {
            dest[j] = src[i];                        
            ++j;
        }
    }

    if (bNow)
    {
        // Making sure destination string is nicely terminated
        dest[++j] = NULL;
    }

}

int NumberOfLinesInWFLFile()
{
     char* szComparer = NULL;
     char szBuffer[100] = ""; 
     int iFirstCountTheLines = 0;

     // FILE* h = fopen("WinForLifeHistory.csv","r"); 
     // For Debug Purpose Only
     FILE* h = fopen("C:\\Documents and Settings\\dromischer\\My Documents\\Visual Studio 2005\\Projects\\Wfl\\Debug\\WinForLifeHistory.csv","r");    

   if (h == NULL)
   {
       printf("error occured reading file = %d \n", errno);
       return (NULL);       
   }
   else
   {
       bool bStart = false;

       while (fgets(szBuffer, 100, h))
       {
           // At first run, i only count the lines in order to hold the array data
           // structure
           if(bStart)
           {
             szComparer = strstr(szBuffer, ",");
             if(szComparer)
             {
                 ++iFirstCountTheLines;
             }
           }
           else
           {
               // Look for Draw date, if it exist, then 
               // the next line starts containing the winning numbers
               szComparer = strstr(szBuffer, "Draw Date");

               if(szComparer)
               {
                    bStart = true;
                    continue;
               }
           }
       }
   }
   fclose(h);

   // For Debugging Purpose
   // printf("NUMBER OF LINES = %d \n", iFirstCountTheLines);

   return(iFirstCountTheLines);
}


bool ReadInWFLFile(int outMatrix[][PIK_WFL_VALUE + 2], int size)
{         
   char* szComparer = NULL;
   char szBuffer[100] = ""; 

   if(size)
   {
        
       // FILE* h = fopen("WinForLifeHistory.csv","r"); 
        // For Debug Purpose Only
        FILE* h = fopen("C:\\Documents and Settings\\dromischer\\My Documents\\Visual Studio 2005\\Projects\\Wfl\\Debug\\WinForLifeHistory.csv","r");    
        
        if (h == NULL)
        {
            printf("error occured reading file = %d \n", errno);
            return (false);
        }
        else
        {
            bool bStart = false;
            int counter = 0;
            while (fgets(szBuffer, 100, h) && (counter < size))
            {
                if(bStart)
                {
                   
                    char tempstr[100] = "";
                    char numberstr[100] = "";
                    sscanf(szBuffer,"%s", tempstr);

                    // After doing the sscanf above the date is still there
                    // so i must stripe out the numbers
                    stripeNumbers(tempstr, numberstr);
                    
                    // Now actually start reading in the data into the Matrix
                    sscanf( numberstr , "%02d-%02d-%02d-%02d-%02d-%02d" , &outMatrix[counter][0], &outMatrix[counter][1],
                        &outMatrix[counter][2], &outMatrix[counter][3], &outMatrix[counter][4], &outMatrix[counter][5]);

                    // For Debugging Purpose:
                    // printf("%02d-%02d-%02d-%02d-%02d-%02d\n", outMatrix[counter][0],outMatrix[counter][1],outMatrix[counter][2],outMatrix[counter][3],outMatrix[counter][4],outMatrix[counter][5]);                    
                    // printf("%s \n", numberstr);

                    ++counter;
                }
                else
                {
                    // Look for Draw date, if it exist, then 
                    // the next line starts containing the winning numbers
                    szComparer = strstr(szBuffer, "Draw Date");

                    if(szComparer)
                    {
                        bStart = true;
                        continue;
                    }
                }
            }
        }
        fclose(h);

        return(true);
   }
   else
   {
    return(false);
   }

}


void CalculateNumberOfTimesPlayed(int iMatrix[][PIK_WFL_VALUE + 2],int size,int outValues[MAX_WFL_VALUE])
{

   int Value = 0;

   // Iterates thru the lottery array and calculates all the occurences of numbers
   for(int i = 0; i < size; i++)
   {
        for(int j = 0; j < PIK_WFL_VALUE; j++)
        {
            Value = iMatrix[i][j];
            outValues[Value-1] = ++outValues[Value-1];
        }       
   }
        
    if(bAnalysisMode && !bNumbersPlayedAlreadyPrinted)
    {    
        printf("General PrintStats for all Numbers:\n");
        for(int p = 0; p < MAX_WFL_VALUE; p++)   
        {
            printf("Number %02d has been played %d \n", p+1, outValues[p]);
        }
        printf("\n");

        bNumbersPlayedAlreadyPrinted = true;
    }
    
}

void CalculateFatheredPlayed(int iMatrix[][PIK_WFL_VALUE + 2],int size, int outValues[PIK_WFL_VALUE])
{

   int Value = 0;

   // Iterates thru the lottery array (fathered only)
   // and calculates all the occurences of numbers
   for(int i = 0; i < size; i++)
   {
       Value = iMatrix[i][PIK_WFL_VALUE];
       outValues[Value] = ++(outValues[Value]);        
   }

    if(bAnalysisMode)
    {
       
        for(int p = 0; p < PIK_WFL_VALUE; p++)   
        {
            printf("Fathered %02d has occured %d times\n", p, outValues[p]);
        }
        printf("\n");        
    }
    
}

void CalculateGrandfatheredPlayed(int iMatrix[][PIK_WFL_VALUE + 2],int size, int outValues[PIK_WFL_VALUE])
{

   int Value = 0;

   // Iterates thru the lottery array (fathered only)
   // and calculates all the occurences of numbers
   for(int i = 0; i < size; i++)
   {
       Value = iMatrix[i][PIK_WFL_VALUE + 1];
       outValues[Value] = ++(outValues[Value]);        
   }

    if(bAnalysisMode)
    {
        printf("\n");
        for(int p = 0; p < PIK_WFL_VALUE; p++)   
        {
            printf("Grandfathered %02d has occured %d times\n", p, outValues[p]);
        }
        printf("\n");        
    }
    
}

void calculateSevenMostHitNumbers(int iMatrix[][PIK_WFL_VALUE + 2],int size, int outSevenMostHit[7])
{   
   int Values[MAX_WFL_VALUE] = {0};   
   
   if(!bValuesAlreadyCalculated)
   {       
       CalculateNumberOfTimesPlayed(iMatrix,size,arrValues);
       bValuesAlreadyCalculated = true;
   }
   
   // Copy the array over because I'll modify the values   
   copyInArr(Values, arrValues, MAX_WFL_VALUE);      

   // Now we have all the occurance, now i still need to get the top most seven
   int max = 0;
   int maxIndex = 0;
   int tempval = 0;

   for (int k = 0; k < 7; k++)
   {
        for(int i = 0; i < MAX_WFL_VALUE; i++)
        {
            tempval = Values[i];
            if(tempval > max)
            {
                max = tempval;
                maxIndex = i;
            }
        }

        //reset the MaxValue so it won't be included
        //in next iteration & also reset max        
        Values[maxIndex] = 0;
        max = 0;

        //send out the actual number value
        outSevenMostHit[k] = maxIndex + 1;
   }

}

void calculateSevenLeastHitNumbers(int iMatrix[][PIK_WFL_VALUE + 2],int size, int outSevenLeastHit[7])
{

   const int SOME_HIGH_NUMBER = 9999;
   int Values[MAX_WFL_VALUE] = {0};

   if(!bValuesAlreadyCalculated)
   {       
       CalculateNumberOfTimesPlayed(iMatrix,size,arrValues);
       bValuesAlreadyCalculated = true;
   }
   
   // Copy the array over because I'll modify the values   
   copyInArr(Values, arrValues, MAX_WFL_VALUE);

   // Now we have all the occurance, now i still need to get the top most seven
   int min = SOME_HIGH_NUMBER; //initialized to some max value
   int minIndex = 0;
   int tempval = 0;

   INDEX_LEAST = SIZE_CALC;

   for (int k = 0; k < 7; k++)
   {
        for(int i = 0; i < MAX_WFL_VALUE; i++)
        {
            tempval = Values[i];
            if(tempval < min)
            {
                min = tempval;
                minIndex = i;
            }
        }

        //reset the MaxValue so it won't be included
        //in next iteration & also reset max        
        Values[minIndex] = SOME_HIGH_NUMBER; //setting it to some high number
        min = SOME_HIGH_NUMBER;

        //send out the actual number value
        outSevenLeastHit[k] = (minIndex + 1);

        //Add the value to the Global Buffer                
        arrCalculated[SIZE_CALC++] = (minIndex + 1);
        ++SIZE_LEAST;
   }

}

unsigned int calculateAverageAmountEachNumberIsPlayed(int iMatrix[][PIK_WFL_VALUE + 2],int size)
{
   if(!bValuesAlreadyCalculated)
   {       
       CalculateNumberOfTimesPlayed(iMatrix,size,arrValues);
       bValuesAlreadyCalculated = true;
   }
   
   unsigned int Average = 0;

   for(int i; i < MAX_WFL_VALUE; i++)
   {
        Average += arrValues[i];
   }    

   return((Average / MAX_WFL_VALUE));
}

float calculateStdDeviation(int iMatrix[][PIK_WFL_VALUE + 2],int size)
{
    //Calculating std deviation as described in
    //http://hubpages.com/hub/stddev

    // We need the average, so it must have been already calculated elsewhere /i keep it global,
    // eventhough it is bad practice
    if(iPlayedAverage)
    {
        int Values[MAX_WFL_VALUE] = {0};   
        int Deviation = 0;        
        float tempcalc = 0;

        if(!bValuesAlreadyCalculated)
        {       
            CalculateNumberOfTimesPlayed(iMatrix,size,arrValues);
            bValuesAlreadyCalculated = true;
        }
   
        // Copy the array over because I'll modify the values   
        copyInArr(Values, arrValues, MAX_WFL_VALUE);


        // Step2: generate list of deviations
        // Debug Purpose ONLY:
        //printf("List of Deviations:\n"); 
        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            Values[i] = Values[i] - iPlayedAverage;
            //printf("Value[%02d]=%02d\n", i , Values[i]); 
        }

        // Step3: square of deviations
        // Debug Purpose ONLY:
        //printf("Square of Deviations:\n"); 
        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            Values[i] = pow((double)Values[i], 2 );
            //printf("Value[%02d]=%02d\n", i , Values[i]);             
        }

        // Step4: sum of deviations        
        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            Deviation = Deviation + Values[i];
        }
        // Debug Purpose ONLY:
        //printf("Sum of Deviations: %d\n", Deviation); 

        //Step5: divide by one less than the number of items
        Deviation = (Deviation / (MAX_WFL_VALUE - 1));
        // Debug Purpose ONLY:
        //printf("Divided by one Less: %d\n", Deviation); 

        //Step6: take the square root of that number
        tempcalc = sqrt((double) Deviation);

        //printf("Result: %d\n", (int) tempcalc); 

        return ((float)tempcalc);

    }

    return(0);
}

void calculateFathered(int iMatrix[][PIK_WFL_VALUE + 2], int size)
{
    int valtolookfor = 0;
    int valtoinc = 0;

    // Since all the values are undefined, first set all fathered
    // values to zero
    for (int i = size - 2; i >= 0; i--)
    {
        iMatrix[i][PIK_WFL_VALUE] = 0;
    }

    // Going backwards thru the index of the matrix starting at end
    // of index - 1, because those are the only numbers that have a 
    // father (the last number doesn't!)
    for (int i = size - 2; i >= 0; i--)
    {
        //iterate thru the indexed and the fathered array/ find all
        //the values that are equal and add them to the 6th field of
        //the matrix (specially reserved for fathered calculations
        for(int j = 0; j < PIK_WFL_VALUE; j++)
        {
            valtolookfor = iMatrix[i][j];

            // We are looking for fathered so increment index by 1
            if(IsInArr(valtolookfor, iMatrix[i + 1], PIK_WFL_VALUE))
            {
                // if found increment the fathered value by 1
                valtoinc = iMatrix[i][PIK_WFL_VALUE];
                iMatrix[i][PIK_WFL_VALUE] = ++valtoinc;                                
            }

        }

    }

}

void calculateGrandfathered(int iMatrix[][PIK_WFL_VALUE + 2], int size)
{
    int valtolookfor = 0;
    int valtoinc = 0;

    // Since all the values are undefined, first set all fathered
    // values to zero
    for (int i = size - 3; i >= 0; i--)
    {
        iMatrix[i][PIK_WFL_VALUE + 1] = 0;
    }

    // Going backwards thru the index of the matrix starting at end
    // of index - 1, because those are the only numbers that have a 
    // grandfather (the last 2 numbers dosn't!)
    for (int i = size - 3; i >= 0; i--)
    {
        //iterate thru the indexed and the fathered array/ find all
        //the values that are equal and add them to the 6th field of
        //the matrix (specially reserved for fathered calculations
        for(int j = 0; j < PIK_WFL_VALUE; j++)
        {
            valtolookfor = iMatrix[i][j];

            // We are looking for fathered so increment index by 1
            if(IsInArr(valtolookfor, iMatrix[i + 2], PIK_WFL_VALUE))
            {
                // if found increment the Grandfathered value by 1
                valtoinc = iMatrix[i][PIK_WFL_VALUE + 1];
                iMatrix[i][PIK_WFL_VALUE + 1] = ++valtoinc;                                
            }

        }

    }
}

void calculateFatherChecksum(int iMatrix[][PIK_WFL_VALUE + 2])
{       

    for(int i = 0; i < PIK_WFL_VALUE; i++)
    {
        iFatherChecksum = iFatherChecksum + iMatrix[0][i];
    }

}
void calculateAndShowGrandFatherAndFatherStats(int iMatrix[][PIK_WFL_VALUE + 2], int size)
{
    int BothAreZero = 0;
    int FatherZeroGFOne = 0;
    int FatherOneGFZero = 0;
    int BothAreOne = 0;

     for (int i = 0; i < size - 2; i++)
    {
        if( (iMatrix[i][PIK_WFL_VALUE] == 0) && (iMatrix[i][PIK_WFL_VALUE + 1] == 0))
        {
            ++BothAreZero;
        }
        else if( (iMatrix[i][PIK_WFL_VALUE] == 0) && (iMatrix[i][PIK_WFL_VALUE + 1] == 1))
        {
            ++FatherZeroGFOne;
        }
        else if( (iMatrix[i][PIK_WFL_VALUE] == 1) && (iMatrix[i][PIK_WFL_VALUE + 1] == 0))
        {
            ++FatherOneGFZero;
        }
        else if ((iMatrix[i][PIK_WFL_VALUE] == 1) && (iMatrix[i][PIK_WFL_VALUE + 1] == 1))
        {
            ++BothAreOne;
        }

    }

     float odds = ((float) BothAreZero / (size - 2));
     printf("Fathered and GrandFathered are BOTH zero, odds: %02.2f \n", ((float)odds * 100));
     odds = ((float) FatherZeroGFOne / (size - 2));
     printf("Fathered is 0 and GrandFathered is 1, odds: %02.2f \n", ((float)odds * 100));     
     odds = ((float) FatherOneGFZero / (size - 2));
     printf("Fathered is 1 and GrandFathered is 0, odds: %02.2f \n", ((float)odds * 100));     
     odds = ((float) BothAreOne / (size - 2));
     printf("Fathered and GrandFathered are BOTH one, odds: %02.2f \n", ((float)odds * 100));     
}

void OddEvenAnalysis(int iMatrix[][PIK_WFL_VALUE + 2], int size)
{
    int odd[PIK_WFL_VALUE + 1] = {0};
    int oddcount = 0;

    for(int i = 0; i < size; i++)
    {
        oddcount = 0;
        for(int j = 0; j < PIK_WFL_VALUE; j++)
        {
            if(isOdd(iMatrix[i][j]))
            {
                ++oddcount;
            }

        }
        odd[oddcount] = ++(odd[oddcount]);
    }

    printf("\nOddEven Analysis: \n");
    for (int k = 0; k < (PIK_WFL_VALUE + 1); ++k)
    {
        printf("Odd%02d occured %02d times \n", k, odd[k]);           
    }
    float odds = ((float)odd[2]/size);
    printf("\nPercentage of getting odd2 is %02.2f percent\n", ( odds * 100));
    odds = ((float)odd[3]/size);
    printf("Percentage of getting odd3 is %02.2f percent\n", ( odds * 100));
    odds = ((float)odd[4]/size);
    printf("Percentage of getting odd4 is %02.2f percent\n\n", ( odds * 100));
}

void RecentlyPlayedStats(int iMatrix[][PIK_WFL_VALUE + 2], int size, int outValues[MAX_WFL_VALUE])
{
   
   int Value = 0;
   int j = 0;
   int k = 0;

   // Iterates thru the lottery array and calculates all the occurences of numbers
   for(int i = 0; i < size; i++)
   {
        j = i / PIK_WFL_VALUE;
        k = i%PIK_WFL_VALUE;
        
        Value = iMatrix[j][k];
        outValues[Value-1] = ++outValues[Value-1];
   }

}

void PrintStats(char* statTitle, int* inStatValues, int inStatSize)
{
    //Only Print the ones that have yet not been played:
    
    printf("PrintStats for %s:\n", statTitle);

    for (int i = 0; i < inStatSize; i++)
    {
        if(inStatValues[i] == 0)
        {
            printf("Number %02d has not been played\n", i+1);
        }        
    }
    printf("\n");

}

bool CheckNumberSetEvenOddRatio(int number, int* arr, int arrSize)
{
    
    int countOdd = 0;
    int countEven = 0;
    const int MAX_NUM_OF_EVENS = PIK_WFL_VALUE - MAX_NUM_OF_ODDS;

    int pikCounter = arrSize%PIK_WFL_VALUE;
    
    for(int i = arrSize - pikCounter; i < arrSize; ++i)
    {
        int n = arr[i];

        if(isOdd(n))
        {
            ++countOdd;
        }
        else
        {
            ++countEven;
        }
    }

    if((countOdd == MAX_NUM_OF_ODDS) && isOdd(number))
    {
        return(false);
    }
    else if((countEven == MAX_NUM_OF_EVENS) && !isOdd(number))    
    {
        return(false);
    }
    else
    {
        return(true);
    }    
}

bool CheckNumberEvenOddRatio(int number, int* arr, int arrSize, int OddRatio)
{
    
    int countOdd = 0;
    int countEven = 0;
    const int MAX_NUM_OF_EVENS = PIK_WFL_VALUE - OddRatio;

    int pikCounter = arrSize%PIK_WFL_VALUE;
    
    for(int i = arrSize - pikCounter; i < arrSize; ++i)
    {
        int n = arr[i];

        if(isOdd(n))
        {
            ++countOdd;
        }
        else
        {
            ++countEven;
        }
    }

    if((countOdd == OddRatio) && isOdd(number))
    {
        return(false);
    }
    else if((countEven == MAX_NUM_OF_EVENS) && !isOdd(number))    
    {
        return(false);
    }
    else
    {
        return(true);
    }    
}


unsigned int GenerateSeed(int _seed)
{       

    int minrange_Seed = 3;
    int maxrange_Seed = ( (float) MAX_WFL_VALUE / 16) * 10;        // ~65% of max value
    SYSTEMTIME st;    

    if (!_seed)
    {
        do
        {
            GetLocalTime(&st);     
            if (bUseFatherInSeedGeneration)
            {
                Seed = (st.wMilliseconds / st.wDay) % iFatherChecksum;
            }
            else
            {
                Seed = st.wMilliseconds % st.wDay;
            }
            Milliseconds = st.wMilliseconds;
        }
        while ((Seed < minrange_Seed) || (Seed > maxrange_Seed));    
    }
    else
    {
        Seed = _seed;
    }
    
    return(Seed);
}

int GenerateRandomNumber(int RANGE_MIN, int RANGE_MAX)
{
    int RandomNumber = 0;    
    if(bGeneratingRandomNumberForFirstTime)
    {
        Seed = GenerateSeed(USE_SEED);
        srand(Seed);
    
        // Calling rand() once - the first time always seems generates a 1
        RandomNumber = (((double) rand() /
                     (double) RAND_MAX) * RANGE_MAX + RANGE_MIN);

        bGeneratingRandomNumberForFirstTime = false;
    }   

   do
   {
        // Calling rand() - otherwise ONLY call it once
        RandomNumber = (((double) rand() /
                     (double) RAND_MAX) * RANGE_MAX + RANGE_MIN);
   }
   while ((RandomNumber < RANGE_MIN) || (RandomNumber > RANGE_MAX));

   return(RandomNumber);
}

// Depreciated, Won't use that function anymore
void GenerateInitialRandomLotteryArray(int InitRandomArray[MAX_WFL_VALUE])
{
    int arrSize = 0;
    int RandomNumber = 0;
    int arrNumbersShuffled[MAX_WFL_VALUE] = {0};
    int arrMaximizedChance[MAX_WFL_VALUE] = {0};

    while(arrSize < MAX_WFL_VALUE)
    {
        // Generating the Random Number within MIN and MAX range
        RandomNumber = GenerateRandomNumber(MIN_WFL_VALUE, MAX_WFL_VALUE);

        if(IsInArr(RandomNumber, arrNumbersShuffled, arrSize))
        {
            continue;
        }
        else
        {
            arrNumbersShuffled[arrSize] = RandomNumber;
            ++arrSize;
        }
    }        

    //Maximize My Chances (by randomizing one more time, and using
    //those values as the index values into the arrNumbersShuffled
    int iLuck = 0;
    while(iLuck < MAX_WFL_VALUE)
    {
        // Generating the Random Number within MIN and MAX range        
        RandomNumber = GenerateRandomNumber(MIN_WFL_VALUE, MAX_WFL_VALUE);

        if(IsInArr(RandomNumber, arrMaximizedChance, iLuck))
        {
            continue;
        }
        else
        {
            arrMaximizedChance[iLuck] = RandomNumber;
            ++iLuck;
        }
    }

    bool HappyAndRich = false;
    int wflSize = 0;
    bool bDebug = false;

    // Build the *Winning* Lottery Ticket :)
    for(int iWin = 0; !HappyAndRich; ++iWin)
    {   

        int serendipityCounter = iWin%MAX_WFL_VALUE;
        
        //I don't store a value for zero so i need to subtract one to get to all 
        //the values     
        int serendipityIndex = arrMaximizedChance[serendipityCounter] - 1; 
        int serendipityNumber = arrNumbersShuffled[serendipityIndex];

        if(IsInArr(serendipityNumber, InitRandomArray, wflSize))
        {            
            continue;
        }
        else
        {
            //maximize my chances by only having 3 odds and 3 evens in my number set
            bool bInsertOk = CheckNumberSetEvenOddRatio(serendipityNumber, InitRandomArray, wflSize);

            if (bInsertOk)
            {            

                InitRandomArray[wflSize] = serendipityNumber;;
               ++wflSize;

               if(wflSize == MAX_WFL_VALUE)
               {
                   HappyAndRich = true;
               }

            }

        }

    }

}

// Depreciated, Won't use that function anymore
void DistributeNumbersRandomlyIntoTicket(int* arrWinningTicket, int* src, int srcSize)
{

    int RandomDest = 0;
    int RandomSrc = 0;

    //Use the TopSeven in the winning TicketCalculations
    for(int i = 0; i < MAX_WFL_VALUE; i = i + PIK_WFL_VALUE)
    {
        // Generating the Random Number within the Src
        RandomSrc = GenerateRandomNumber(1, srcSize);                       

        // Generating the Random Number within 1 to PIK_WFL_VALUE!
        RandomDest = GenerateRandomNumber(1, PIK_WFL_VALUE);

        WasFound:
        // Keep Generating until a random location is found that fits
        // the odd / even ration  - we must subtract 1 to stay within 0 - 6! or 0 - 5 respectively
        while( (isOdd(src[RandomSrc - 1])) != (isOdd(arrWinningTicket[i + RandomDest - 1])))
        {
            // Generating another Random Number within 1 to 6
            RandomDest = GenerateRandomNumber(1, PIK_WFL_VALUE);            
        }

        if(IsInArr(src[RandomSrc - 1], arrWinningTicket + i, PIK_WFL_VALUE))
        {
            //The number is already on that ticket, so we need another number
            //At this point i am generating another srcNumber instead of another
            //DestNumber
            RandomSrc = GenerateRandomNumber(1, srcSize);
            goto WasFound;
        }
        else
        {
            // we can assign the number now :)
            arrWinningTicket[i + RandomDest - 1] = src[RandomSrc - 1];
        }

    }

}

void ShowNumbersWithinDeviation(int whatDeviationLevelToShow)
{
    
    int StdDevRangeMin = iPlayedAverage - (whatDeviationLevelToShow * iStdDeviation);
    int StdDevRangeMax = iPlayedAverage + (whatDeviationLevelToShow * iStdDeviation);
        
    int StdDevUntilMin = iPlayedAverage - iStdDeviation;
    int StdDevUntilMax = iPlayedAverage + iStdDeviation;
    
    int cmp = 0;    

    if (whatDeviationLevelToShow)
    {   
        if (whatDeviationLevelToShow == 1)
        {
            INDEX_FirstMean = SIZE_CALC;
        }
        else if(whatDeviationLevelToShow == 2)
        {
            INDEX_SecondMean = SIZE_CALC;
        }                               

        int j = 0;

        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            cmp = arrValues[i];

            if (whatDeviationLevelToShow == 1)
            {
                if  ((cmp >= StdDevRangeMin) &&                     
                     (cmp <= StdDevRangeMax)                      
                    )
                    {
                        printf("%02d ", i+1);
                        ++j;                        

                        // store globally
                        arrCalculated[SIZE_CALC++] = i+1;
                        ++SIZE_FirstMean;

                        // Show 10 numbers per line
                        if((j != 0) && !(j%9))
                        {
                            printf("\n");
                        }
                    }
            }
            else if (whatDeviationLevelToShow == 2)
            {
                  if  (((cmp >= StdDevRangeMin) &&
                       (cmp < StdDevUntilMin)  
                       ) 
                       ||
                       ((cmp <= StdDevRangeMax) &&
                        (cmp > StdDevUntilMax)
                        )
                    )
                    {
                        printf("%02d ", i+1);
                        ++j;
                        
                        // store globally
                        arrCalculated[SIZE_CALC++] = i+1;
                        ++SIZE_SecondMean;

                        // Show 10 numbers per line
                        if((j != 0) && !(j%9))
                        {
                            printf("\n");
                        }
                    }
            }                       
            
        }
        

    }   
}


void AddLastFatherAndLastGrandFatherValuesIntoCalculationBuffer(int iMatrix[][PIK_WFL_VALUE + 2], int size)
{
    
    int value = 0;
    INDEX_Father = SIZE_CALC;    

    for(int i = 0; i < PIK_WFL_VALUE; i++)
    {
        value = iMatrix[0][i];
        arrCalculated[SIZE_CALC++] = value;
        ++SIZE_Father;
    }

    INDEX_GrandFather = SIZE_CALC;

    for(int i = 0; i < PIK_WFL_VALUE; i++)
    {
        value = iMatrix[1][i];
        arrCalculated[SIZE_CALC++] = value;
        ++SIZE_GrandFather;
    }

}

void ShuffleInCalcArray(int Index, int Size)
{
    int RandomNumber1 = 0;
    int RandomNumber2 = 0;
    int swap = 0;

    for(int i = Size; i > 0; i--)
    {
        RandomNumber1=GenerateRandomNumber(1, Size);
        RandomNumber2=GenerateRandomNumber(1, Size);

        swap = arrCalculated[Index + RandomNumber1 - 1];
        arrCalculated[Index + RandomNumber1 - 1] = arrCalculated[Index + RandomNumber2 - 1];
        arrCalculated[Index + RandomNumber2 - 1] = swap;
    }

}

void InsertRandomlyIntoWinningTicket(int arrCalcIndex, int arrCalcSize, int n)
{
    int num = 0;
    int Index = 0;
    int RandomNumber = 0;
    int NumberToInsert = 0;

    // For Father
    ShuffleInCalcArray(arrCalcIndex, arrCalcSize);
    for (int i = 0; i < n; i++)
    {
        RegenerateIt:
        RandomNumber = GenerateRandomNumber(1, arrCalcSize);
        NumberToInsert = arrCalculated[arrCalcIndex + RandomNumber - 1];

        do
        {
            RandomNumber = GenerateRandomNumber(1, MAX_WFL_VALUE);
            Index = RandomNumber - 1;
            num = arrWinningTicket[Index];            
        }
        while(isOdd(NumberToInsert) != isOdd(num));        

        // BUG check if it is in the array FIRST before inserting it,
        // if it is then regenerate another number (goto)                                     
        if(IsInArr(NumberToInsert, arrWinningTicket + Index - (Index%PIK_WFL_VALUE), PIK_WFL_VALUE))
        {
            goto RegenerateIt;
        }
        else
        {
            arrWinningTicket[Index] = NumberToInsert;
        }
    }
}

//Generate the Winning Lottery Ticket:
//New strategy is... fill ticket randomly (all 42 slots with the 33 1st mean values)
//~3 even/odd
//~2 even2/odd4
//~2 even4/odd2
//Then randomly fill 3 fathered values, and 4 grandfathered values
//Then pick 3 least values, 2 2nd mean Diviation values, 2 warm values, and 1 hot value and distribute, that's it!
void FillWinningTicketWithNumbersAccordingly()
{
    int RandomNumber = 0;
    int TicketCounter = 0;          //initializing to first ticket
    int NumberToInsert = 0;

    // Before generating the First Mean, i am shuffling the numbers, i think that is
    // a good idea, a) to make it more random, b) i think it avoids that one bug i have
    // that the same numbers are generated over and over because the numbers in that array
    // are basically in order.
    ShuffleInCalcArray(INDEX_FirstMean, SIZE_FirstMean);

    for(int i = 0; i < MAX_WFL_VALUE; i++)
    {
       if(i != 0)
       {
           if(!(i % PIK_WFL_VALUE)) ++TicketCounter;
       }

       // We only want to put randomly generated numbers in that fit within the Odd Even
       // Ratio that we want
       Regenerate:
       RandomNumber = GenerateRandomNumber(1, SIZE_FirstMean);
       NumberToInsert = arrCalculated[INDEX_FirstMean + RandomNumber - 1];

       if(TicketCounter < 3)
       {
            if(CheckNumberEvenOddRatio(NumberToInsert, arrWinningTicket, i , 3))
            {                
                if(IsInArr(NumberToInsert, arrWinningTicket + i - (i%PIK_WFL_VALUE), (i%PIK_WFL_VALUE)))
                {
                    goto Regenerate;
                }
                else
                {
                    arrWinningTicket[i] =  NumberToInsert;
                }
            }
            else
            {
                goto Regenerate;
            }
       }
       else if(TicketCounter < 5)
       {
            if(CheckNumberEvenOddRatio(NumberToInsert, arrWinningTicket, i , 2))
            {               
                if(IsInArr(NumberToInsert, arrWinningTicket + i - (i%PIK_WFL_VALUE), (i%PIK_WFL_VALUE)))
                {
                    goto Regenerate;
                }
                else
                {
                    arrWinningTicket[i] =  NumberToInsert;
                }
            }
            else
            {
                goto Regenerate;
            }
       }
       else if(TicketCounter < 7)
       {
            if(CheckNumberEvenOddRatio(NumberToInsert, arrWinningTicket, i , 4))
            {
                if(IsInArr(NumberToInsert, arrWinningTicket + i - (i%PIK_WFL_VALUE), (i%PIK_WFL_VALUE)))
                {
                    goto Regenerate;
                }
                else
                {
                    arrWinningTicket[i] =  NumberToInsert;
                }
            }
            else
            {
                goto Regenerate;
            }
       }             

    }

    // NOW FINALLY i can do this:
    //Then randomly fill 3 fathered values, and 4 grandfathered values
    //Then pick 3 least values, 2 2nd mean Diviation values, 2 warm values, and 1 hot value and distribute, that's it!

    // For Father    
    InsertRandomlyIntoWinningTicket(INDEX_Father, SIZE_Father, NUM_OF_FATHER_VALUES_TO_USE);
    
    // For GrandFather
    InsertRandomlyIntoWinningTicket(INDEX_GrandFather, SIZE_GrandFather, NUM_OF_GRANDFATHER_VALUES_TO_USE);

    // For Least Values
    InsertRandomlyIntoWinningTicket(INDEX_LEAST, SIZE_LEAST, NUM_OF_LEAST_VALUES_TO_USE);    

    // For 2nd Mean Std. Diviation
    InsertRandomlyIntoWinningTicket(INDEX_SecondMean, SIZE_SecondMean, NUM_OF_SECOND_MEAN_VALUES_TO_USE); 
        
    // For Warm values
    InsertRandomlyIntoWinningTicket(INDEX_Warm, SIZE_Warm, NUM_OF_WARM_VALUES_TO_USE);

    // For Hot Values
    InsertRandomlyIntoWinningTicket(INDEX_Hot, SIZE_Hot, NUM_OF_HOT_VALUES_TO_USE);

}

//int main(array<System::String ^> ^args)
int main(int argc, char* argv[])
{
    bAnalysisMode = true;

    // We are in analysis mode so just print out all the analysis of all the tickets
    int Lotsize = NumberOfLinesInWFLFile();
    int (*iMatrix)[PIK_WFL_VALUE + 2]= new int[Lotsize][PIK_WFL_VALUE + 2];

    if(ReadInWFLFile(iMatrix, Lotsize))
    {
        // use FatherChecksum to calculate Seed :) ~genius
        if(bUseFatherInSeedGeneration)
        {
            calculateFatherChecksum(iMatrix);
        }

        printf("\n");
        printf("Winning Lottery Numbers so far: (%d count): \n\n", Lotsize);
        for (int counter = 0; counter < Lotsize; counter++)
        {
            printf("%02d-%02d-%02d-%02d-%02d-%02d\n", iMatrix[counter][0],iMatrix[counter][1],iMatrix[counter][2],iMatrix[counter][3],iMatrix[counter][4],iMatrix[counter][5]);                    
        }            

        // Print out odd / Matrix:
        OddEvenAnalysis(iMatrix, Lotsize);

        // Calculate the odds of the recently playes numbers,
        // we only look at the last 1 strata (of numbers)
        int strata1values[MAX_WFL_VALUE] = {0};        
        RecentlyPlayedStats(iMatrix, MAX_WFL_VALUE, strata1values);
        PrintStats("Strata1 Values (WARM)", strata1values, MAX_WFL_VALUE);

        // Calculate the odds of the recently playes numbers,
        // we only look at the last 1.5 strata (of numbers)
        int strata15values[MAX_WFL_VALUE] = {0};                
        RecentlyPlayedStats(iMatrix, (MAX_WFL_VALUE + (MAX_WFL_VALUE / 2)), strata15values);
        PrintStats("Strata1.5 Values (VERY WARM)", strata15values, MAX_WFL_VALUE);
        // Insert Warm numbers into Global Calculation Buffer
        INDEX_Warm = SIZE_CALC;
        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            if(strata15values[i] == 0)
            {
                arrCalculated[SIZE_CALC++] = (i+1);
                ++SIZE_Warm;
            }
        }

        // Calculate the odds of the recently playes numbers,
        // we only look at the last 2 strata (of numbers)
        int strata2values[MAX_WFL_VALUE] = {0};                
        RecentlyPlayedStats(iMatrix, (MAX_WFL_VALUE + MAX_WFL_VALUE), strata2values);
        PrintStats("Strata2 Values (HOT)", strata2values, MAX_WFL_VALUE);
        // Insert Hot numbers into Global Calculation Buffer
        INDEX_Hot = SIZE_CALC;
        for (int i = 0; i < MAX_WFL_VALUE; i++)
        {
            if(strata2values[i] == 0)
            {
                arrCalculated[SIZE_CALC++] = (i+1);
                ++SIZE_Hot;
            }
        }

        // Calculate the odds of the recently playes numbers,
        // we only look at the last 2 strata (of numbers)
        int strata25values[MAX_WFL_VALUE] = {0};                
        RecentlyPlayedStats(iMatrix, (MAX_WFL_VALUE + MAX_WFL_VALUE + (MAX_WFL_VALUE / 2)), strata25values);
        PrintStats("Strata2.5 Values (ON FIRE)", strata25values, MAX_WFL_VALUE);

        // Calculate the odds of the recently playes numbers,
        // we only look at the last 2 strata (of numbers)
        int strata3values[MAX_WFL_VALUE] = {0};                
        RecentlyPlayedStats(iMatrix, (MAX_WFL_VALUE + MAX_WFL_VALUE + MAX_WFL_VALUE), strata3values);
        PrintStats("Strata3 Values (GONE NUCLEAR)", strata3values, MAX_WFL_VALUE);

        // Calculate the Top 7 Most found numbers!
        int TopSeven[7] = {0};
        calculateSevenMostHitNumbers(iMatrix,Lotsize, TopSeven);

        printf("The Seven Most Played Numbers are:\n");            
        for (int counter = 0; counter < 7; counter++)
        {
            printf("%02d ", TopSeven[counter]);                    
        }
        printf("\n\n");

        // Calculate the Least 7 found numbers!
        int LeastSeven[7] = {0};
        calculateSevenLeastHitNumbers(iMatrix,Lotsize, LeastSeven);

        printf("The Seven Least Played Numbers are:\n");            
        for (int counter = 0; counter < 7; counter++)
        {
            printf("%02d ", LeastSeven[counter]);                    
        }
        printf("\n\n");

        iPlayedAverage = calculateAverageAmountEachNumberIsPlayed(iMatrix,Lotsize);
        printf("The Average Amount each number was played: %02d\n\n", iPlayedAverage);

        fStdDeviation = calculateStdDeviation(iMatrix,Lotsize);
        printf("The Std. Deviation for all the winning numbers is: %02.2f\n", fStdDeviation);
        double ceilStdDeviation = ceil((double) fStdDeviation);
        iStdDeviation = (int) ceilStdDeviation; 
        printf("The Std. Deviation Ceiling for all the winning numbers is: %d \n\n", iStdDeviation);

        printf("Showing all the numbers within Std. Deviation 1:\n");
        ShowNumbersWithinDeviation(1);
        printf("\n\nShowing all the numbers within Std. Deviation 2:\n");
        ShowNumbersWithinDeviation(2);        
        

        ////
        // Fathered 
        ////
        calculateFathered(iMatrix,Lotsize);

        printf("\nFather / Grandfather Calculations (.5 Strata) \n");
        int FatheredValues[PIK_WFL_VALUE] = {0};
        CalculateFatheredPlayed(iMatrix, (Lotsize - 1), FatheredValues);

        int highestoccurringFatheredNumber = 0;
        MaxInArr(FatheredValues, PIK_WFL_VALUE, highestoccurringFatheredNumber);                        
        float odds = ((float)FatheredValues[0]/(Lotsize - 1));
        printf("Fathered 0 odds: %02.2f percent \n", (float)(odds * 100));
        odds = ((float)FatheredValues[1]/(Lotsize - 1));
        printf("Fathered 1 odds: %02.2f percent \n", (float)(odds * 100));

        iFatheredAverage = highestoccurringFatheredNumber;
        printf("The Highest occuring Fathered Average for all the winning numbers is: %02d\n", iFatheredAverage);

        ////
        // Grandfathered 
        ////
        calculateGrandfathered(iMatrix,Lotsize);

        int GrandfatheredValues[PIK_WFL_VALUE] = {0};
        CalculateGrandfatheredPlayed(iMatrix, (Lotsize - 2), GrandfatheredValues);

        int highestoccurringGrandfatheredNumber = 0;
        MaxInArr(GrandfatheredValues, PIK_WFL_VALUE, highestoccurringGrandfatheredNumber);
        odds = ((float)GrandfatheredValues[0]/(Lotsize - 2));
        printf("Grandfathered 0 odds: %02.2f percent \n", (float)(odds * 100));
        odds = ((float)GrandfatheredValues[1]/(Lotsize - 2));
        printf("Grandfathered 1 odds: %02.2f percent \n", (float)(odds * 100));

        iGrandfatheredAverage = highestoccurringGrandfatheredNumber;
        printf("The Highest occuring Grandfathered Average for all the winning numbers is: %02d\n\n", iGrandfatheredAverage);

        //Show all Combined Father and GrandFather Stats:
        calculateAndShowGrandFatherAndFatherStats(iMatrix, Lotsize);        

        // Add Father and GrandFather to calculation Buffer
        AddLastFatherAndLastGrandFatherValuesIntoCalculationBuffer(iMatrix, Lotsize);

       //Generate the Winning Lottery Ticket:
       //New strategy is... fill ticket randomly (all 42 slots with the 33 1st mean values)
       //Then randomly fill 3 fathered values, and 4 grandfathered values
       //Then pick 3 least values, 2 2nd mean Diviation values, 2 warm values, and 1 hot value and distribute, that's it!
       FillWinningTicketWithNumbersAccordingly();

       //Write the Ticket to File:
       WriteOut(arrWinningTicket, MAX_WFL_VALUE, Seed);
    }
        
    return 0;
}
