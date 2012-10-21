#include "StdAfx.h"
#include "Generator.h"

Generator::Generator(void):
bIsFirstTime(true),
m_seedInitializer(1),
m_seedUsedCounter(1),
m_seed(1)
{

}

Generator::Generator(unsigned int initNum):
bIsFirstTime(true),
m_seedInitializer(initNum),
m_seedUsedCounter(1),
m_seed(1)
{
    // pass in what ever checksum here [optional]
    /*if(bUseFatherInSeedGeneration)
        {
            calculateFatherChecksum(iMatrix);
        }*/
}

Generator::~Generator(void)
{

}

unsigned int Generator::GenerateSeed(const unsigned int SeedInitialzer)
{       

    unsigned int Seed = 0;
    const unsigned int minrange_Seed = WFL_NUMBERS_DRAWN_PER_GAME * WFL_MAX_DIGIT_VALUE;

    SYSTEMTIME st;    

    do
    {        
        Seed = (SeedInitializer)? SeedInitializer, 1;
        Seed = (Seed * st.wMilliseconds) % st.wDay; 
        Seed = (Seed * 1103515245 +12345);
        Seed = (Seed / 65536) % 32768;
    }
    while ((Seed < minrange_Seed));    
    
    return(Seed);
}

unsigned int Generator::GenerateRandomNumber(const unsigned int RANGE_MIN, const unsigned int RANGE_MAX)
{
    unsigned int RandomNumber = 0;    
    if(bIsFirstTime || !(m_seedUsedCounter % RANGE_MAX))
    {
        m_seedUsedCounter = 1;
        m_seed = GenerateSeed(m_seedInitializer);        
        srand(m_seed);

        // Calling rand() once - the first time always seems generates a 1
        RandomNumber = (((double) rand() /
            (double) RAND_MAX) * RANGE_MAX + RANGE_MIN);

        bIsFirstTime = false;
    }   

    do
    {        
        RandomNumber = (((double) rand() /
            (double) RAND_MAX) * RANGE_MAX + RANGE_MIN);
    }
    while ((RandomNumber < RANGE_MIN) || (RandomNumber > RANGE_MAX));

    ++m_seedUsedCounter;
    return(RandomNumber);
}

void Generator::GenerateRandomSetFromSet(int* destSet, const int destSize, const int* srcSet, const int srcSize)
{

    int n = 0;

    for (int i = 0; i < destSize; i++)
    {
        n = GenerateRandomNumber(1,srcSize);
        destSet[i] = srcSet[--n];
    }

}
