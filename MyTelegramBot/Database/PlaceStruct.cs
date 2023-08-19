public static class PlaceStruct
{
    public static Place GetPlace(string message)
    {
        return message switch
        {
            "DgapDormitory" => Place.DgapDormitory,
            "MainBuilding" => Place.MainBuilding,
            _ => throw new NullReferenceException("Place not found")
        };
    }
}