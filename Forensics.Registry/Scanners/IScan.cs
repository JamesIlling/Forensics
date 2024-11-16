namespace Forensics.Registry.Scanners;

public interface IScan<T>
{
    List<T> Scan();
}