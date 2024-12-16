namespace Forensics.Registry.Scanners;

public interface IScan<T>
{
    public string Name { get; }
    List<T> Scan();
}