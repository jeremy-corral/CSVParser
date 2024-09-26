using System;
using System.Collections.Generic;
using System.IO;

class CSVParser
{
    // Stores the parsed rows of the CSV file
    public List<List<string>> Rows { get; private set; } = new List<List<string>>();

    // Constructor accepts the CSV file path and parses it
    public CSVParser(string filePath)
    {
        ParseCSV(filePath);
    }

    // Reads the CSV file line by line and parses each line
    private void ParseCSV(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var row = ParseLine(line);
                    Rows.Add(row); // Add parsed row to the list
                }
            }
        }
    }

    // Parses an individual CSV line, handling quoted fields and commas
    private List<string> ParseLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        string currentField = "";

        for (int i = 0; i < line.Length; i++)
        {
            char currentChar = line[i];

            if (inQuotes)
            {
                if (currentChar == '"')
                {
                    // Check for double quotes (escaped quote)
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField += '"'; // Add escaped quote
                        i++; // Skip the next quote
                    }
                    else
                    {
                        inQuotes = false; // End of quoted field
                    }
                }
                else
                {
                    currentField += currentChar; // Add current character to field
                }
            }
            else
            {
                if (currentChar == '"')
                {
                    inQuotes = true; // Start of quoted field
                }
                else if (currentChar == ',')
                {
                    fields.Add(currentField.Trim()); // Add completed field
                    currentField = ""; // Reset for next field
                }
                else
                {
                    currentField += currentChar; // Continue building the field
                }
            }
        }

        fields.Add(currentField.Trim()); // Add the last field
        return fields;
    }

    // Print all parsed CSV data
    public void PrintParsedData()
    {
        foreach (var row in Rows)
        {
            Console.WriteLine(string.Join(" , ", row));
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string filePath = "sample.csv"; // Path to the CSV file
        CSVParser parser = new CSVParser(filePath);
        parser.PrintParsedData(); // Print the parsed CSV content
    }
}