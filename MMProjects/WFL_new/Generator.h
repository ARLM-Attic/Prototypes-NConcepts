#pragma once

class Generator
{
public:

////
// Constructors / Destructors
////
    Generator(void);
    Generator(unsigned int initNum);
    ~Generator(void);

////
// Public Methods
////
    unsigned int GenerateRandomNumber(const unsigned int RANGE_MIN, const unsigned int RANGE_MAX);
    void GenerateRandomSetFromSet(int* destSet, const int destSize, const int* srcSet, const int srcSize)

private:
    bool bIsFirstTime;
    unsigned int m_seedUsedCounter;
    unsigned int m_seedInitializer;
    unsigned int m_seed;
    unsigned int GenerateSeed(const unsigned int SeedInitializer = 1);

};
