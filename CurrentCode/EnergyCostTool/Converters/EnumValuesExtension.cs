using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;

namespace EnergyCostTool.Converters;

public class EnumValuesExtension : MarkupExtension
{
    private readonly Type enumType;

    public EnumValuesExtension(Type enumType)
    {
        if (enumType == null || !enumType.IsEnum)
            throw new ArgumentException("Invalid enum type");

        this.enumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(e => new { Value = e, Description = GetEnumDescription(e) })
            .ToArray();
    }

    private static string GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        Debug.Assert(fieldInfo != null, nameof(fieldInfo) + " != null");
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        return descriptionAttribute?.Description ?? value.ToString();
    }
}