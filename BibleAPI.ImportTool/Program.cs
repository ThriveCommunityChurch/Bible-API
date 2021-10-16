using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BibleAPI.Core;
using System.Text;
using System.IO;

namespace BibleAPI.ImportTool
{
    static class Program
    {
        private static Dictionary<string, int> books = new Dictionary<string, int>()
            {
                { "Genesis",  50 },
                { "Exodus", 40 },
                { "Leviticus",  27 },
                { "Numbers",    36 },
                { "Deuteronomy",    34 },
                { "Joshua", 24 },
                { "Judges", 21 },
                { "Ruth",   4 },
                { "1 Samuel",   31 },
                { "2 Samuel",    24 },
                { "1 Kings", 22 },
                { "2 Kings", 25 },
                { "1 Chronicles",    29 },
                { "2 Chronicles",    36 },
                { "Ezra",   10 },
                { "Nehemiah",   13 },
                { "Esther", 10 },
                { "Job",    42 },
                { "Psalms", 150 },
                { "Proverbs",   31 },
                { "Ecclesiastes",   12 },
                { "Song of Solomon",    8 },
                { "Isaiah", 66 },
                { "Jeremiah",   52 },
                { "Lamentations",   5 },
                { "Ezekiel",    48 },
                { "Daniel", 12 },
                { "Hosea",  14 },
                { "Joel",   3 },
                { "Amos",   9 },
                { "Obadiah",    1 },
                { "Jonah",  4 },
                { "Micah",  7 },
                { "Nahum",  3 },
                { "Habakkuk",   3 },
                { "Zephaniah",  3 },
                { "Haggai", 2 },
                { "Zechariah",  14 },
                { "Malachi",    4 },
                { "Matthew",    28 },
                { "Mark",   16 },
                { "Luke",   24 },
                { "John",   21   },
                { "Acts",   28 },
                { "Romans", 16 },
                { "1 Corinthians",  16 },
                { "2 Corinthians",  13 },
                { "Galatians",  6 },
                { "Ephesians",  6 },
                { "Philippians",    4 },
                { "Colossians", 4 },
                { "1 Thessalonians",    5 },
                { "2 Thessalonians",    3 },
                { "1 Timothy",  6 },
                { "2 Timothy",  4 },
                { "Titus",  3 },
                { "Philemon",   1 },
                { "Hebrews",    13 },
                { "James",  5 },
                { "1 Peter", 5 },
                { "2 Peter", 3 },
                { "1 John",  5 },
                { "2 John",  1 },
                { "3 John",  1 },
                { "Jude",   1 },
                { "Revelation", 22 }
            };

        static void Main(string[] args)
        {
            var superscriptChars = new List<char> 
            {
                '\u2070',
                '\u00b9',
                '\u00b2',
                '\u00b3',
                '\u2074',
                '\u2075',
                '\u2076',
                '\u2077',
                '\u2078',
                '\u2079'
            };

            var filePath = "S:\\BibleAPI\\";
            var booksObjs = new List<Book>();
            int bookNum = 1;

            Parallel.ForEach(books, book =>
            {
                var passageObjs = new List<Passage>();

                for (int chapter = 1; chapter <= book.Value; chapter++)
                {
                    var newBook = new Book
                    {
                        BookNumber = (byte)bookNum,
                        Name = book.Key
                    };

                    var escapedString = Uri.EscapeDataString(book.Key + chapter);
                    var uri = string.Format("http://BLAH?searchCriteria={0}", escapedString);

                    var response = GetAsync(uri);
                    var text = response.Content.ReadAsStringAsync().Result;
                    int verseNum = 0;

                    text = text.Replace("{\"Passage\":\"", "");
                    text = text.Replace("}", "");

                    StringBuilder sb = new StringBuilder();

                    // we need to go through each character and determine if we are looking at the next verse
                    for (int i = 0; i < text.Length; i++)
                    {
                        var character = text.ElementAt(i);
                        if ($"{character}" == "\n")
                        {
                            continue;
                        }

                        if (superscriptChars.Contains(character))
                        {
                            verseNum++;
                            if (verseNum < 10)
                            {
                                var nextChar = text.ElementAt(i + 1);
                                if (superscriptChars.Contains(nextChar))
                                {
                                    verseNum--;
                                    continue;
                                }
                                else if (character == '\u00b9')
                                {
                                    sb.Clear();
                                    verseNum--;
                                    continue;
                                }
                                else
                                {
                                    passageObjs.Add(new Passage
                                    {
                                        Language = LanguageCode.English,
                                        Text = sb.ToString().Trim(),
                                        VerseNumber = verseNum,
                                        Version = VersionCode.ESV
                                    });
                                    sb.Clear();
                                }
                            }
                            else if (sb.ToString() != "")
                            {
                                passageObjs.Add(new Passage
                                {
                                    Language = LanguageCode.English,
                                    Text = sb.ToString().Trim(),
                                    VerseNumber = verseNum,
                                    Version = VersionCode.ESV
                                });
                                sb.Clear();
                            }
                        }
                        else
                        {
                            sb.Append(character);
                        }
                    }

                    //open file stream
                    using (StreamWriter file = File.CreateText($"{filePath}{book.Key}_{chapter}.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, passageObjs);
                    }

                    bookNum++;
                }
            });
        }

        private static HttpResponseMessage GetAsync(string url, string authToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "HttpClient");

            // we may not ever have an auth token in the request
            if (!string.IsNullOrEmpty(authToken))
            {
                request.Headers.Add("Authorization", authToken);
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.Send(request);

            return response;
        }
    }
}