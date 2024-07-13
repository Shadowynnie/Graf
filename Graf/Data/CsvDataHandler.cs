using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graf.Data
{
    internal class CsvDataHandler
    {
        // Method to read CSV file and return data as a list of string arrays
        public List<string[]> ReadCsv(string filePath)
        {
            List<string[]> csvData = new List<string[]>();
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        csvData.Add(values);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
            }
            return csvData;
        }

        // Method to write data to a CSV file
        public void WriteCsv(string filePath, List<string[]> data)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var line in data)
                    {
                        var lineString = string.Join(",", line);
                        writer.WriteLine(lineString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the CSV file: {ex.Message}");
            }
        }

        // Method to process CSV data (example: filter rows where first column value is "example")
        public List<string[]> ProcessData(List<string[]> data)
        {
            List<string[]> processedData = data
                .Where(row => row.Length > 0 && row[0].Equals("example", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return processedData;
        }
    }
}
