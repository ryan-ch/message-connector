using System;

namespace XB.MtParser.Enums
{
    public static class EnumUtil
    {
        public static T ParseEnum<T>(string value, T defaultValue) where T : struct
        {
            return Enum.TryParse(value, true, out T enumValue) && Enum.IsDefined(typeof(T), enumValue)
                ? enumValue
                : defaultValue;
        }
    }
}
