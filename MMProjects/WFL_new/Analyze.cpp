#include "StdAfx.h"
#include "Analyze.h"

Analyze::Analyze(void)
{
    Initialize();

    // At first run, i only count the lines in order to correctly allocate the array data
    // structure
    m_iMatrixSize = LinesInWFLFile(WFL_FILE_NAME_AND_PATH);
    
    // Now allocate the Structure
    m_iMatrix = new int[m_iMatrixSize][WFL_NUMBERS_DRAWN_PER_GAME + CUSTOM_FIELDS];
    m_iMatrix2 = m_iMatrix;

    // Now read in entire file into the newly created array
    ReadInWFLFile(WFL_FILE_NAME_AND_PATH);

    // Calculate any custom Fields (useful for later calculations)
    CalculateCustomFields();
}

Analyze::Initialize()
{
    m_arrNumberOfTimesPlayed = {0};
    m_arrOddEvenRatio = {0};
    m_fStdDeviation = 0;
    m_arrCalcBufferSize = 0;
}

Analyze::~Analyze(void)
{
    delete [] m_iMatrix;
}

void Analyze::AnalyzeThis(int startIndex, int FinalIndex)
{
    // Always do this one first
    calculateNumberOfTimesPlayed();
    
    Average = calculateAverageAmountEachNumberIsPlayed();
    m_fStdDeviation = calculateStdDeviation();
    
    // assign the ceiling value to public property
    double ceilStdDeviation = ceil((double) fStdDeviation);
    StdDeviation = (int) ceilStdDeviation; 


}

int  Analyze::GetSize()
{
    return (m_iMatrixSize);
}
    
inline int Analyze::GetSize(bool bUseMatrix2)
{

    if(bUseMatrix)
    {
        return m_iMatrixSize2;
    }
    else
    {
        return m_iMatrixSize;
    }
}

inline WFL_Matrix GetPointer(bool bUseMatrix2)
{

    if(bUseMatrix)
    {
        return m_iMatrix;
    }
    else
    {
        return m_iMatrix2;
    }
}

inline void Analyze::stripeNumbers(const char* src, char* dest)
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

inline void Analyze::PrintStrataStats(const char* statTitle, const int* inStatValues, const int inStatSize)
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

inline void Analyze::calculateGF(int GF)
{
    
    int valtolookfor = 0;
    int valtoinc = 0;

    // Since all the values are undefined, first set all fathered
    // values to zero
    for (int i = m_iMatrixSize - (2 + GF); i >= 0; i--)
    {
        m_iMatrix[i][WFL_NUMBERS_DRAWN_PER_GAME + GF] = 0;
    }

    // Going backwards thru the index of the matrix starting at end
    // of index - 1, because those are the only numbers that have a 
    // father (the last number doesn't!)
    for (int i = m_iMatrixSize - (2 + GF); i >= 0; i--)
    {
        
        //iterate thru the indexed and the fathered array/ find all
        //the values that are equal and add them to the 6th field of
        //the matrix (specially reserved for fathered calculations
        for(int j = 0; j < WFL_NUMBERS_DRAWN_PER_GAME; j++)
        {
            
            valtolookfor = m_iMatrix[i][j];

            // We are looking for fathered so increment index by 1
            if(u_IsInArr(valtolookfor, m_iMatrix[i + 1], WFL_NUMBERS_DRAWN_PER_GAME))
            {
                // if found increment the fathered value by 1
                valtoinc = m_iMatrix[i][WFL_NUMBERS_DRAWN_PER_GAME + GF];
                m_iMatrix[i][WFL_NUMBERS_DRAWN_PER_GAME + GF] = ++valtoinc;                                
            }
        }
    }
}

void inline Analyze::CopyStrataValuesIntoCalcBuffer(int inValues[MAX_WFL_VALUE], int Index)
{
   m_arrIndexBuffer[Index] = m_arrCalcBufferSize;
   
   for (int i = 0; i < MAX_WFL_VALUE; i++)
   {
       
       if (inValues[i] == 0)
       {
           m_arrCalcBuffer[m_arrCalcBufferSize++] = (++i);
           m_arrSizeBuffer[Index] += 1;
       }
   }
}

int Analyze::LinesInWFLFile(char* szFileName)
{
    char* szComparer = NULL;
    char szBuffer[100] = ""; 
    int iFirstCountTheLines = 0;

    FILE* h = fopen(szFileName,"r");

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
            // At first run, i only count the lines in order to correctly allocate the array data
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

// This Functions prints out all the statistics generated in this class to stdout for analysis
void Analyze::PrintStatistics()
{
    ////
    // Winning Numbers so far
    ////
    printf("\n");
    printf("Winning Lottery Numbers so far: (%d count): \n\n", Lotsize);
    for (int i = 0; i < m_iMatrixSize; i++)
    {
        for (int j = 0; j < WFL_NUMBERS_DRAWN_PER_GAME; j++)
        {
            if((!j) && !(j%(WFL_NUMBERS_DRAWN_PER_GAME - 1)))
            {
                printf("%02d\n", m_iMatrix[i][j];
            }
            else
            {
                printf("%02d-", m_iMatrix[i][j];
            }
        }
    }
    
    ////
    // General Played Number stats
    ////
     printf("General PrintStats for all Numbers:\n");
     for(int i = 0; i < WFL_MAX_DIGIT_VALUE; i++)   
     {
        printf("Number %02d has been played %d \n", ++i, m_arrNumberOfTimesPlayed[i]);
     }
     printf("\n");

     ////
    // Odd Even Analysis / Statistics
    ////
    printf("\nOddEven Analysis: \n");
    for (int i = 0; i < (WFL_NUMBERS_DRAWN_PER_GAME + 1); ++i)
    {
        printf("Odd%02d occured %02d times \n", i, m_arrOddEvenRatio[i]);           
    }
    float odds = ((float)m_arrOddEvenRatio[2]/size);
    printf("\nPercentage of getting odd2 is %02.2f percent\n", ( odds * 100));
    odds = ((float)m_arrOddEvenRatio[3]/size);
    printf("Percentage of getting odd3 is %02.2f percent\n", ( odds * 100));
    odds = ((float)m_arrOddEvenRatio[4]/size);
    printf("Percentage of getting odd4 is %02.2f percent\n\n", ( odds * 100));

    ////
    // Std deviation:
    ////
    printf("The Std. Deviation for all the winning numbers is: %02.2f\n", fStdDeviation);
    printf("The Std. Deviation Ceiling for all the winning numbers is: %d \n\n", iStdDeviation);

    ////
    // Print Strata Statistics
    ////
    PrintStats("Strata1 Values (WARM)", m_arrStrataWarm, MAX_WFL_VALUE);
    PrintStats("Strata1.5 Values (VERY WARM)", m_arrStrataVeryWarm, MAX_WFL_VALUE);
    PrintStats("Strata2 Values (HOT)", m_arrStrataHot, MAX_WFL_VALUE);
    PrintStats("Strata2.5 Values (ON FIRE)", m_arrStrataOnFire, MAX_WFL_VALUE);
    PrintStats("Strata3 Values (GONE NUCLEAR)", m_arrStrataGoneNuclear, MAX_WFL_VALUE);

}



bool Analyze::ReadInWFLFile(char* FileName)
{         
   char* szComparer = NULL;
   char szBuffer[100] = ""; 

   if(m_iMatrixSize)
   {               
        FILE* h = fopen(szFileName,"r");    
        
        if (h == NULL)
        {
            printf("error occured reading file = %d \n", errno);
            return (false);
        }
        else
        {
            bool bStart = false;
            int counter = 0;
            while (fgets(szBuffer, 100, h) && (counter < m_iMatrixSize))
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
                    sscanf( numberstr , "%02d-%02d-%02d-%02d-%02d-%02d" , &m_iMatrix[counter][0], &m_iMatrix[counter][1],
                        &m_iMatrix[counter][2], &m_iMatrix[counter][3], &m_iMatrix[counter][4], &m_iMatrix[counter][5]);

                    // For Debugging Purpose:
                    // printf("%02d-%02d-%02d-%02d-%02d-%02d\n", m_iMatrix[counter][0],m_iMatrix[counter][1],m_iMatrix[counter][2],m_iMatrix[counter][3],m_iMatrix[counter][4],m_iMatrix[counter][5]);                    
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



bool Analyze::CalculateCustomFields()
{
    // Calculate Father and GrandFather Values    
    calculateGF(cusFIELD_FATHER);
    calculateGF(cusFIELD_GRANDFATHER);

    // Calculate Single and Doubles

}

float Analyze::calculateStdDeviation()
{
    //Calculating std deviation as described in
    //http://hubpages.com/hub/stddev

    // We need the average, so it must have been already calculated elsewhere /i keep it global,
    // eventhough it is bad practice
    if(Average)
    {
        int Values[WFL_MAX_DIGIT_VALUE] = {0};   
        int Deviation = 0;        
        float tempcalc = 0;
  
        // Copy the array over because I'll modify the values   
        copyInArr(Values, m_arrNumberOfTimesPlayed, WFL_MAX_DIGIT_VALUE);


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

unsigned int Analyze::calculateAverageAmountEachNumberIsPlayed()
{

    unsigned int Average = 0;

    for(int i = 0; i < WFL_MAX_DIGIT_VALUE; i++)
    {
        Average += m_arrNumberOfTimesPlayed[i];
    }    

    return((Average / WFL_MAX_DIGIT_VALUE));
}

void calculateOddEvenRatio()
{
      
    int oddcount = 0;

    for(int i = 0; i < m_iMatrixSize; i++)
    {
        oddcount = 0;
        for(int j = 0; j < WFL_NUMBERS_DRAWN_PER_GAME; j++)
        {
            if(isOdd(m_iMatrix[i][j]))
            {
                ++oddcount;
            }

        }
        m_arrOddEvenRatio[oddcount] = ++(m_arrOddEvenRatio[oddcount]);
    } 

}

void Analyze::calculateNumberOfTimesPlayed()
{

   int Value = 0;

   // Iterates thru the lottery array and calculates all the occurences of numbers
   for(int i = 0; i < m_iMatrixSize; i++)
   {
        for(int j = 0; j < WFL_NUMBERS_DRAWN_PER_GAME; j++)
        {
            Value = m_iMatrix[i][j];
            m_arrNumberOfTimesPlayed[Value-1] = ++m_arrNumberOfTimesPlayed[Value-1];
        }       
   }                
}

void Analyze::calculateRecentlyPlayedStats(int outValues[MAX_WFL_VALUE], int size, bool bUseMatrix2)
{

   WFL_Matrix iMatrix = GetPointer(bUserMatrix2);   

   int Value = 0;
   int j = 0;
   int k = 0;

   // Iterates thru the lottery array and calculates all the occurences of numbers
   for(int i = 0; i < size; i++)
   {
        j = i / WFL_NUMBERS_DRAWN_PER_GAME;
        k = i%WFL_NUMBERS_DRAWN_PER_GAME;
        
        Value = iMatrix[j][k];
        outValues[Value-1] = ++outValues[Value-1];
   }

}

void Analyze::calculateAllStratas()
{
    
    // base position for strata calculations
    m_iMatrix2 = m_iMatrix + 1;

    RecentlyPlayedStats(m_arrStrataWarm, WFL_MAX_DIGIT_VALUE, true);
    RecentlyPlayedStats(m_arrStrataVeryWarm, (WFL_MAX_DIGIT_VALUE + (WFL_MAX_DIGIT_VALUE / 2)), true);
    RecentlyPlayedStats(m_arrStrataHot, (WFL_MAX_DIGIT_VALUE + WFL_MAX_DIGIT_VALUE), true);
    RecentlyPlayedStats(m_arrStrataOnFire, (WFL_MAX_DIGIT_VALUE + WFL_MAX_DIGIT_VALUE + (WFL_MAX_DIGIT_VALUE / 2)), true);
    RecentlyPlayedStats(m_arrStrataGoneNuclear, (WFL_MAX_DIGIT_VALUE + WFL_MAX_DIGIT_VALUE + WFL_MAX_DIGIT_VALUE), true);

    if (bUseMatrix2)
    {
        CopyStrataValuesIntoCalcBuffer(m_arrStrataWarm, indexWarm);
        CopyStrataValuesIntoCalcBuffer(m_arrStrataVeryWarm, indexVeryWarm);
        CopyStrataValuesIntoCalcBuffer(m_arrStrataHot, indexHot);
        CopyStrataValuesIntoCalcBuffer(m_arrStrataOnFire, indexOnFire);
        CopyStrataValuesIntoCalcBuffer(m_arrStrataGoneNuclear, indexGoneNuclear);
    }
            
}

