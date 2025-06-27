using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.Essentials;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace FinalProject
{
    public class DB
    {
        private static string DBName = "log.db";
        public static SQLiteConnection conn;
        public static void OpenConnection()
        {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, DBName);
            conn = new SQLiteConnection(fname);
            conn.CreateTable<DessertInfo>();
            conn.CreateTable<IngredientInfo>();
            conn.CreateTable<InstructionInfo>();
            conn.CreateTable<History>();
            TableQuery<DessertInfo> dessertTable = conn.Table<DessertInfo>();
            TableQuery<IngredientInfo> ingredientTable = conn.Table<IngredientInfo>();
            TableQuery<InstructionInfo> instructionTable = conn.Table<InstructionInfo>();
            if (dessertTable.FirstOrDefault() == null)
                PopulateDessertDB();
            if (ingredientTable.FirstOrDefault() == null)
                PopulateIngredientDB();
            if (instructionTable.FirstOrDefault() == null)
                PopulateInstructionDB();
        }

        private static void PopulateDessertDB()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("FinalProject.Dessert.json");
                using (StreamReader input = new StreamReader(stream))
                {
                    string jsonString = input.ReadToEnd();
                    JObject str = JObject.Parse(jsonString);
                    foreach (var country in str)
                    {
                        foreach (JToken dessert in country.Value)
                        {
                            DessertInfo newDessert = new DessertInfo();
                            newDessert.CountryName = country.Key;
                            newDessert.DessertID = Int32.Parse(dessert["DessertID"].ToString());
                            newDessert.DessertName = dessert["Dessert Name"].ToString();
                            newDessert.PrepTime = dessert["Prep Time"].ToString();
                            newDessert.ServingSize = dessert["Serving Size"].ToString();
                            newDessert.CookTime = dessert["Cooking Time"].ToString();
                            newDessert.Description = dessert["Description"].ToString();
                            newDessert.Image = dessert["Image"].ToString();
                            newDessert.CountryFlag = dessert["Country Flag"].ToString();
                            conn.Insert(newDessert);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private static void PopulateIngredientDB()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("FinalProject.Ingredients.json");
                using (StreamReader input = new StreamReader(stream))
                {
                    string jsonString = input.ReadToEnd();
                    JObject str = JObject.Parse(jsonString);
                    JToken ingredients = str["ingredients"];
                    foreach (JToken i in ingredients)
                    {
                        IngredientInfo newIngredient = new IngredientInfo();
                        JProperty ingredient = (JProperty)i.First;
                        newIngredient.Type = ingredient.Name;
                        newIngredient.Content = ingredient.First.ToString();
                        newIngredient.DessertID = Int32.Parse(ingredient.Next.First.ToString());
                        conn.Insert(newIngredient);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private static void PopulateInstructionDB()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("FinalProject.Instructions.json");
                using (StreamReader input = new StreamReader(stream))
                {
                    string jsonString = input.ReadToEnd();
                    JObject str = JObject.Parse(jsonString);
                    JToken instructions = str["instructions"];
                    foreach (JToken i in instructions)
                    {
                        InstructionInfo newInstruction = new InstructionInfo();
                        JProperty instruction = (JProperty)i.First;
                        newInstruction.Step = instruction.Name;
                        newInstruction.Content = instruction.First.ToString();
                        newInstruction.DessertID = Int32.Parse(instruction.Next.First.ToString());
                        conn.Insert(newInstruction);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void DeleteTableContents(string tableName)
        {
            conn.Execute("DROP TABLE " + tableName);
        }
    }
}