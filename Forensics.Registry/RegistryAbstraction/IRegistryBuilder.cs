namespace Forensics.Registry.RegistryAbstraction;

public interface IRegistryBuilder
{
    IRegistry? GetRegistry(string key);
}