namespace Regulus.Serialization
{
    public interface ISerializable
    {
        byte[] Serialize(object instance);
        object Deserialize(byte[] buffer);
    }
}