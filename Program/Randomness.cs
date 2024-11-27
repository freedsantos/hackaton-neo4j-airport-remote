using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    static class Randomness
    {
        public static int passengerVolumeMin = 100;   
        public static int passengerVolumeMax = 5000; 
        public static int cargoCapacityMin = 10;     
        public static int cargoCapacityMax = 200;    
        public static int flightFrequencyMin = 1;    
        public static int flightFrequencyMax = 50;   
        public static double onTimePerformanceMin = 85.0; 
        public static double onTimePerformanceMax = 99.9; 
        public static int delayAverageMin = 0;     
        public static int delayAverageMax = 120;   
        public static int incidentReportsMin = 0;  
        public static int incidentReportsMax = 5;  
    }
}
