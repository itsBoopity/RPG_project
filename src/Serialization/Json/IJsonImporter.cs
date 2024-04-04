public interface IJsonImporter<T>
{
	public T Import(string json);
}