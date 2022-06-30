using CSCI2910_Lab6.Models;
using System.Text.Json;
using ConsoleTables;
using System.Net;
using System.IO;

namespace CSCI2910_Lab6
{
    public class Program
    {
        static void Main(string[] args)
        {
            // path to the JSON files
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
            //var filePath = root + $"{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}oathbringer.json";
            var dataPath = root + $"{Path.DirectorySeparatorChar}Data";
            var outputPath = root + $"{Path.DirectorySeparatorChar}Output";

            // create options JSONSerializer must follow
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var bookList = new List<Book>();
            foreach(string fileName in Directory.GetFiles(dataPath))
            {
                // set JSON string to empty and set up streamreader based on data file's path
                string jsonReadString = string.Empty;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    jsonReadString = sr.ReadToEnd();
                }

                // deserialize (read) JSON and create objects based on its information
                var book = JsonSerializer.Deserialize<Book>(jsonReadString, options);

                bookList.Add(book);
            }

            // display each book object
            //foreach(Book book in bookList)
            //{
                //Console.WriteLine(book);
            //}

            // serialize (write) JSON from the object
            var options2 = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(bookList, options2);

            //Console.WriteLine(jsonString);

            // output to JSON file
            using (StreamWriter sw = new StreamWriter(outputPath + $"{Path.DirectorySeparatorChar}book.JSON", false))
            {
                try
                {
                    sw.WriteLine(jsonString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // ask the user for a book
            //Console.WriteLine("Enter the name of a book: ");
            //var bookString = Console.ReadLine();
            //bookList.Add(JsonSerializer.Deserialize<Book>(GetBookFromAPI(bookString), options));

            // sets up a table with a header
            //ConsoleTable table = new ConsoleTable("Id", "Title", "Authors", "Description", "SelfLink");

            // adds rows to the table
            //foreach (var book in bookList)
            //{
                // will display each book using its toString() method
                //Console.WriteLine(book + "\n");

                //table.AddRow(book.Id, book.VolumeInfo.Title, book.VolumeInfo.Authors, book.VolumeInfo.Description, book.SelfLink);
            //}

            // display the table
            //Console.WriteLine(table);
        }

        // MVC
        static string GetBookFromAPI(string bookName)
        {
            string jsonString = string.Empty;
            var url = $"https://www.googleapis.com/books/v1/volumes?q={bookName}&startIndex=0&maxResults=1";

            // create web request based on URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using(Stream stream = response.GetResponseStream())
                using(StreamReader sr = new StreamReader(stream))
                {
                    jsonString = sr.ReadToEnd();
                }

            return jsonString;
        }
    }
}