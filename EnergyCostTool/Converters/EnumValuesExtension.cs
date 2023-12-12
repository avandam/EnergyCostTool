using System.ComponentModel;
using System.Windows.Markup;

namespace EnergyCostTool.Converters;

public class EnumValuesExtension : MarkupExtension
{
    private readonly Type _enumType;

    public EnumValuesExtension(Type enumType)
    {
        if (enumType == null || !enumType.IsEnum)
            throw new ArgumentException("Invalid enum type");

        _enumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(_enumType)
            .Cast<Enum>()
            .Select(e => new { Value = e, Description = GetEnumDescription(e) })
            .ToArray();
    }

    private static string GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        return descriptionAttribute?.Description ?? value.ToString();
    }
}