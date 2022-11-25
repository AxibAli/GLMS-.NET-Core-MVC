namespace GLMS.Models
{
    public class PropertyConfigurationModel
    {
        public string XmlLabel { get; set; } = "dummy";
        public bool IsRequired { get; set; } = true;
        public string Value { get; set; } = "192.168.1.133";
        public bool IsList { get; set; } = false;
        public ValueCollectionType ValueType { get; set; } = ValueCollectionType.Normal;
        public char Delimeter { get; set; } = ',';
    }
    public enum ValueCollectionType
    {
        Normal = 1,
        List = 2,
        Dictionary = 3,
    }
}
