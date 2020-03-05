using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BudgetManager.Logic
{
    public static class FileHelpers
    {
        public static string GetPdfText(string filename)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(filename))
            {
                PdfReader pdfReader = new PdfReader(filename);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }

        public static string RemoveTrailingNumbers(string line)
        {
            List<string> descripParts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            while (descripParts.Count > 1)
            {
                long.TryParse(descripParts.Last(), out long lastNum);
                if (lastNum > 0)
                {
                    descripParts.RemoveAt(descripParts.Count - 1);
                }
                else
                {
                    break;
                }
            }

            return string.Join(" ", descripParts);
        }

        public static List<string> ParseCSVLine(string line)
        {
            List<string> ret = new List<string>();

            bool inString = false;
            string curEntry = "";
            for (int i = 0; i < line.Length; i++)
            {
                string character = line.Substring(i, 1);
                if (character.Equals("\""))
                {
                    inString = !inString;
                }
                else
                {
                    if (!inString && character.Equals(","))
                    {
                        ret.Add(curEntry);
                        curEntry = "";
                    }
                    else
                    {
                        curEntry = curEntry + character;
                    }
                }
            }

            ret.Add(curEntry);

            return ret;
        }
    }
}
