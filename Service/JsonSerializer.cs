using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ToDoList.Service
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerOptions options;
        public JsonSerializer() 
        { 
            options = new JsonSerializerOptions();
        }
        public JsonSerializer(JsonSerializerOptions options) 
        { 
            this.options = options;
        }
        public void Serialize(string filePath, T value)
        {
            if (Path.GetExtension(filePath).ToLower() != ".json")
                throw new ArgumentException("Destination file is not a .json file!");
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                JsonSerializer.Serialize<T>(stream, value, options);
            }
        }
        public T? Deserialize(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower() != ".json")
                throw new ArgumentException("Source file is not a .json file!");
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return JsonSerializer.Deserialize<T>(stream, options);
            }
        }       
    }
}
