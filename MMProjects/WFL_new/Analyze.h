#pragma once

using namespace AnalyzeSettings;

typedef (int*)[WFL_NUMBERS_DRAWN_PER_GAME + CUSTOM_FIELDS] WFL_Matrix

class Analyze
{
////    
//Constructor/Destructor
////    
public:
    Analyze(void);   
    ~Analyze(void);

////
// Public Methods
////
    void AnalyzeThis(int startIndex, int FinalIndex);
    int GetSize();

////
// Public Members 
//   ~they are initialized to 0, before the first AnalyzeThis call();
////
    int Average;                    // holds the 
    unsigned int StdDeviation;      // i holds the ceiling of fStdDeviation(float value rounded up)

////
// Private Members
////
private:
    void Initialize();              // initializes all member variables and calculation buffers    
    int (*m_iMatrix)[WFL_NUMBERS_DRAWN_PER_GAME + CUSTOM_FIELDS];       // holds all the winning numbers (read from file) plus custom fields
    int (*m_iMatrix2)[WFL_NUMBERS_DRAWN_PER_GAME + CUSTOM_FIELDS];      // secondary pointer used for variable indexing
    int m_iMatrixSize;              // size of iMatrix (also eq to the numbers of winning numbers we have parsed from WFL file)
    int m_iMatrixSize2;             // size of 2ndary matrix; again for variable iteration
    int m_fStdDeviation;    

////
// Private Methods 
////
    bool LinesInWFLFile(char* szFileName);
    int  ReadInWFLFile(char* FileName);
    bool CalculateCustomFields();
    inline int GetSize(bool bUseMatrix2);
    inline WFL_Matrix GetPointer(bool bUseMatrix2);

////
// Private Functions called by AnalyzeThis()
// ~these functions are the heart of this class
////        
    unsigned int calculateAverageAmountEachNumberIsPlayed();
    float calculateStdDeviation();
    void calculateNumberOfTimesPlayed();
    void calculateOddEvenRatio();
    void calculateRecentlyPlayedStats(int outValues[MAX_WFL_VALUE], int size, bool bUseMatrix2 = false)

////    
// calculation Buffers:
////    
    int m_arrNumberOfTimesPlayed[WFL_MAX_DIGIT_VALUE];          // Holds the number of times each number has been played! - Don't modify
    int m_arrOddEvenRatio[WFL_NUMBERS_DRAWN_PER_GAME + 1];      // Holds the number of times Odds are played! - Don't Modify 
                                                                // ~index starts at 1 - 6
    // Strata Values
    int m_arrStrataWarm[WFL_MAX_DIGIT_VALUE];
    int m_arrStrataVeryWarm[WFL_MAX_DIGIT_VALUE];
    int m_arrStrataHot[WFL_MAX_DIGIT_VALUE];
    int m_arrStrataOnFire[WFL_MAX_DIGIT_VALUE];
    int m_arrStrataGoneNuclear[WFL_MAX_DIGIT_VALUE];

    // Imp Calculation Buffer
    int m_arrCalcBuffer[SIZE_OF_CALCULATION_BUFFER];
    int m_arrCalcBufferSize;                                    // important for indexing in correctly
    int m_arrIndexBuffer[NUMBER_OF_INDEXES];                    // this holds the indexes into the calculation buffer
    int m_arrSizeBuffer[NUMBER_OF_INDEXES];                     // this holds the size corresponding to each index into the 
                                                                // calculation buffer

////    
// Helper Functions *functions only created to make functions more concise*
////    
    // called only in ReadInWFLFile()
    inline void stripeNumbers(char* src, char* dest);

    // only called within CalculateCustomFields()
    inline void calculateGF(int GF);

    // Used for Strata Calculations
    void inline CopyStrataValuesIntoCalcBuffer(int inValues[MAX_WFL_VALUE], int Index)

    // only called within PrintStatistics
    inline void PrintStrataStats(const char* statTitle, const int* inStatValues, const int inStatSize);
    
};
