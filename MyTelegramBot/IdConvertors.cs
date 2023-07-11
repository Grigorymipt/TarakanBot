namespace MyTelegramBot.Convertors;
public static class IdConvertor
{
    public static Guid ToGuid(long value)
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}