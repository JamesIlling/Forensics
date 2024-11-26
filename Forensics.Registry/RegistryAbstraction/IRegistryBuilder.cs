namespace Forensics.Registry.RegistryAbstraction;

public interface IRegistryBuilder
{
    IRegistryKey? GetRegistry(string key);
}