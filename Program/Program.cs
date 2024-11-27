namespace Airport
{
    internal class Program
    {
        static string inputFilePath = @"C:\Users\frederico.dos.santos\OneDrive - Avanade\Fred\Documentos\Learning\Neo4J\Case\routes_v3.csv";
        static string outputFilePath = @"C:\Users\frederico.dos.santos\OneDrive - Avanade\Fred\Documentos\Learning\Neo4J\Case\routes_v4.csv";
        static string airportsFilePath = @"C:\Users\frederico.dos.santos\OneDrive - Avanade\Fred\Documentos\Learning\Neo4J\Case\airports.csv";
        
        static void Main(string[] args)
        {
            AddRandomData();
            AddAirportData();
        }

        private static void AddAirportData()
        {
            var airportData = File.ReadAllLines(airportsFilePath)
                .Skip(1) // Skip header line
                .Select(line => line.Split(','))
                .ToDictionary(
                    columns => columns[0], // code (key)
                    columns => new
                    {
                        Name = columns[2],          // name
                        Latitude = columns[3],      // latitude
                        Longitude = columns[4],     // longitude
                        Country = columns[9],       // country
                        City = columns[10],         // city
                        State = columns[11]         // state
                    });

            var routesLines = File.ReadAllLines(inputFilePath);
            var routesHeaders = routesLines[0]; // First line contains headers

            // Define new headers
            var newHeaders = routesHeaders + ",source_name,source_latitude,source_longitude,source_country,source_city,source_state,destination_name,destination_latitude,destination_longitude,destination_country,destination_city,destination_state";

            // Process the routes and add the new columns
            var updatedLines = routesLines
                .Skip(1) // Skip the headers
                .Select(line =>
                {
                    var columns = line.Split(',');
                    string sourceCode = columns[2]; // source_airport
                    string destinationCode = columns[4]; // destination_airport

                    // Lookup data for source and destination airports
                    var sourceAirport = airportData.ContainsKey(sourceCode) ? airportData[sourceCode] : null;
                    var destinationAirport = airportData.ContainsKey(destinationCode) ? airportData[destinationCode] : null;

                    // Add data for source airport
                    var sourceInfo = sourceAirport != null
                        ? $"{sourceAirport.Name},{sourceAirport.Latitude},{sourceAirport.Longitude},{sourceAirport.Country},{sourceAirport.City},{sourceAirport.State}"
                        : ",,,,,,"; // Empty if not found

                    // Add data for destination airport
                    var destinationInfo = destinationAirport != null
                        ? $"{destinationAirport.Name},{destinationAirport.Latitude},{destinationAirport.Longitude},{destinationAirport.Country},{destinationAirport.City},{destinationAirport.State}"
                        : ",,,,,,"; // Empty if not found

                    // Combine original line with new columns
                    return $"{line},{sourceInfo},{destinationInfo}";
                })
                .ToList();

            // Combine headers and updated lines
            updatedLines.Insert(0, newHeaders);

            // Save to a new CSV file
            File.WriteAllLines(outputFilePath, updatedLines);

            Console.WriteLine("Routes CSV file updated with airport details and saved.");
        }

        private static void AddRandomData()
        {
            Random random = new Random();

            var lines = File.ReadAllLines(inputFilePath);

            // Add headers for new columns
            var headers = lines[0] + ",passenger_volume,cargo_capacity,flight_frequency,on_time_performance,delay_average,incident_reports";

            List<string> updatedLines = GetNewLines(random, lines);

            // Combine headers with updated lines
            updatedLines.Insert(0, headers);

            File.WriteAllLines(outputFilePath, updatedLines);

            Console.WriteLine("CSV file has been updated and saved.");
            Console.ReadLine();
        }

        private static List<string> GetNewLines(Random random, string[] lines)
        {
            var updatedLines = lines
                .Skip(1) // Skip the headers
                .Select(line =>
                {
                    // Generate random values for the new columns
                    int passengerVolume = random.Next(Randomness.passengerVolumeMin, Randomness.passengerVolumeMax + 1);
                    int cargoCapacity = random.Next(Randomness.cargoCapacityMin, Randomness.cargoCapacityMax + 1);
                    int flightFrequency = random.Next(Randomness.flightFrequencyMin, Randomness.flightFrequencyMax + 1);
                    double onTimePerformance = Math.Round(random.NextDouble() * (Randomness.onTimePerformanceMax - Randomness.onTimePerformanceMin) + Randomness.onTimePerformanceMin, 2);
                    int delayAverage = random.Next(Randomness.delayAverageMin, Randomness.delayAverageMax + 1);
                    int incidentReports = random.Next(Randomness.incidentReportsMin, Randomness.incidentReportsMax + 1);

                    // Add new values to the line
                    return $"{line},{passengerVolume},{cargoCapacity},{flightFrequency},{onTimePerformance},{delayAverage},{incidentReports}";
                })
                .ToList();

            return updatedLines;
        }
    }
}