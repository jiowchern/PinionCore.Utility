namespace Regulus.Serialization
{
    public interface ISerializable
    {
        byte[] Serialize(System.Type type,object instance);
        object Deserialize(System.Type type, byte[] buffer);
    }

    
}