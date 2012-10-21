// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

// TODO: reference additional headers your program requires here
#include <stdlib.h>
#include <stdio.h>
#include <windows.h>
#include <math.h>
//#include "arrUtils.h"

// Global Settings:
const int WFL_MIN_DIGIT_VALUE               =       1;          // Smallest digit that can be played in WFL
const int WFL_MAX_DIGIT_VALUE               =       42;         // Largest digit that can be played in WFL
const int WFL_NUMBERS_DRAWN_PER_GAME        =       6;          // Numbers drawn each game (Free ball doesn't count)

// TODO: reference additional Namespaces here
namespace GeneratorSettings
{


}

namespace AnalyzeSettings
{

    const char WFL_FILE_NAME_AND_PATH[]         =       ".\WinForLifeHistory.csv";    
    
    // Custom Fields are calculated when the file first is parsed in to speed up 
    // all later calculations (there are stored directly in the same buffer as
    // the winning lottery numbers parsed in from the file)
    const int  CUSTOM_FIELDS                    =       4;

    // The current custom fields (must start at 0)
    // Father + Grandfather must be at 0 and 1, respectively
    // other fields could be set somewhere else
    enum {cusFIELD_FATHER = 0, cusFIELD_GRANDFATHER = 1, cusFIELD_SINGLE = 2, cusFIELD_DOUBLE = 3);

    // Important, the size must be adequate to hold all the calculations
    const int SIZE_OF_CALCULATION_BUFFER        =       200;
    
    // this values corresponds with the number of indexes below
    const int NUMBER_OF_INDEXES                 =       7;

    // This determines how to index into the calculation buffer
    enum {indexWarm = 0, indexVeryWarm = 1, indexHot = 2, indexOnFire = 3, indexGoneNuclear = 4,
          indexTopSeven = 5, indexLeastSeven = 6);
}

namespace PatternSettings
{


}




