using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace D311_Food_File_Import
{
    class Item
    {
        //Hello World!!!! exclamation mark
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public string this[string key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public Dictionary<string, string> Amounts { get; } = new Dictionary<string, string>();
    }

    class Program
    {
        static public string InputFile = @"FoodFiles/2018-01-foodfile.csv";
        static public string OutputFile = @"Results/output.sql";

        static public string AmountTableName = "FoodVariation";
        static public string FoodTableName = "Food";

        static public string[] Columns =
        {
            "Id",
            "Name",
            "Amount",
            "Water",
            "Enegry",
            "EnegryNIP",
            "Protein",
            "Fat",
            "Carbohydrates",
            "DietaryFibre",
            "Sugars",
            "Starch",
            "SFA",
            "MUFA",
            "PUFA",
            "AlphaLinolenicAcid",
            "LinoleicAcid",
            "Cholesterol",
            "Sodium",
            "Iodine",
            "Potassium",
            "Phosphorus",
            "Calcium",
            "Iron",
            "Zinc",
            "Selenium",
            "VitaminA",
            "BetaCarotene",
            "Thiamin",
            "Riboflavin",
            "Niacin",
            "VitaminB6",
            "VitaminB12",
            "DietaryFolate",
            "VitaminC",
            "VitaminD",
            "VitaminE",
        };

        static int Main(string[] args)
        {
            var Command = new RootCommand
            {
                new Option<string>("--input", "Input FoodFile"),
                new Option<string>("--output", "SQL Output")
            };

            Command.Description = "D311 Convert NZ FoodFile To SQL";
            Command.Handler = CommandHandler.Create<string, string>((input, output) =>
            {
                List<Item> items = new List<Item>();
                string sql = "";

                Console.WriteLine($"D311 NZ Food File Import");
                Console.WriteLine($"{"Input File".PadLeft(24)} > {input}");
                Console.WriteLine($"{"Output File".PadLeft(24)} > {output}");
                Console.WriteLine($"");

                RunTask("Reading From FoodFile", () => items = ReadFromCSV(input));
                RunTask("Building Output SQL", () => sql = BuildSql(items));
                RunTask("Saving To File", () => File.WriteAllText(output, sql));
            });

            return Command.InvokeAsync(args).Result;
        }

        /// <summary>
        /// Clean Up All Extra Spaces And Commas, Then Split By Delimiter.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static string[] SanitizeLine(string line)
        {
            line = Regex.Replace(line, @",,", ", ,");
            line = Regex.Replace(line, @",,", ", ,");
            line = Regex.Replace(line, @"^,", " ,");
            line = Regex.Replace(line, @" +", " ");

            return Regex.Matches(line, @"[\""].+?[\""]|[^,]+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToArray(); ;
        }

        /// <summary>
        /// Clean Up All Special Characters For SQL Syntax
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string SanitizeSql(string str)
        {
            str = Regex.Replace(str, @"\""", "");
            str = Regex.Replace(str, @"'", "''");

            return str;
        }

        /// <summary>
        /// Read All Food Items From A Given Food File.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static List<Item> ReadFromCSV(string filename)
        {
           List<Item> _Result = new List<Item>();

            using (var reader = new StreamReader(filename))
            {
                Item item = null;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = SanitizeLine(line);

                    // Check That The Current Line Is A Food Item
                    if (Regex.IsMatch(values[0], @"[A-Z][0-9]{1,6}"))
                    {
                        item = new Item();

                        for (int i = 0; i < Math.Min(values.Length, Columns.Length); i++)
                        {
                            item[Columns[i]] = values[i];
                        }

                        _Result.Add(item);
                    }

                    // Check That The Current Item Is Set
                    else if (item != null && values[0] != "" && values[2] != " ")
                    {
                        item.Amounts[values[1]] = values[2];
                    }

                    // Unset Current Item
                    else
                    {
                        item = null;
                    }
                }
            }

            return _Result;
        }

        /// <summary>
        /// Build The SQL From The Given Items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        static string BuildSql(List<Item> items)
        {
            string ItemSql = "";
            string AmountSql = "";

            ItemSql += $"INSERT INTO '{FoodTableName}' ({String.Join(", ", Columns)}) VALUES";
            AmountSql += $"INSERT INTO '{AmountTableName}' (Id, FoodId, Name, Amount) VALUES";

            using (var progress = new ProgressBar())
            {
                for (int i = 0; i < items.Count; i++)
                {
                    progress.Report((double)i / items.Count);

                    var item = items[i];
                    var id = SanitizeSql(item["Id"]);

                    // Build Item SQL
                    ItemSql += "\n\t(";
                    foreach (var col in Columns)
                    {
                        string itmValue = SanitizeSql(item[col]);
                        string colValue = IsNullOrWhiteSpace(itmValue) ? "0" : itmValue;
                        string prefix = (col == "Id" ? "" : ", ");

                        if (colValue == "trace")
                            colValue = "-1";

                        if (Regex.IsMatch(colValue, @"^[0-9.-]*$"))
                            ItemSql += $"{prefix}{colValue}";
                        else
                            ItemSql += $"{prefix}'{colValue}'";
                    }
                    ItemSql += "),";

                    // Build Amount SQL
                    foreach (var amt in item.Amounts)
                    {
                        string colValue = IsNullOrWhiteSpace(amt.Value) ? "0" : amt.Value;

                        if (colValue == "trace")
                            colValue = "-1";

                        AmountSql += $"\n\t('{Guid.NewGuid().ToString().Substring(2, 8)}', '{id}', '{amt.Key}', {colValue}),";
                    }
                }
            }

            ItemSql = ItemSql.Substring(0, ItemSql.Length - 1) + ";";
            AmountSql = AmountSql.Substring(0, AmountSql.Length - 1) + ";";

            return $"{ItemSql}\n\n{AmountSql}";
        }

        /// <summary>
        /// Check If A String Is A Null Value, Or Contains Spaces
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return true;
            }

            return String.IsNullOrEmpty(value.Trim());
        }

        /// <summary>
        /// Task Delegate
        /// </summary>
        delegate void Task();

        /// <summary>
        /// Run A Named Task
        /// </summary>
        /// <param name="name">Task Name</param>
        /// <param name="task">Task Function</param>
        static void RunTask(string name, Task task)
        {
            Console.Write($"{name.PadLeft(24)} > ");
            task();
            Console.WriteLine("Done!");
        }
    }
}
