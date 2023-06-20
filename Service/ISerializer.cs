namespace ToDoList.Service
{
    public interface ISerializer<T>
    {
        public void Serialize(string filePath, T value);
        public T? Deserialize(string filePath);
    }
}
