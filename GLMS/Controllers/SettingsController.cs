using Microsoft.AspNetCore.Mvc;
using GLMS.Models;

namespace GLMS.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            //var config = CustomLibrary.Utility.xmlConfigMain;
            //var settings = config.GetType().GetProperties().Where(
            //    prop => Attribute.IsDefined(prop, typeof(PropertyConfigAttribute)))
            //    .Select(x => new PropertyConfigurationModel
            //    {
            //        XmlLabel = x.GetCustomAttribute<PropertyConfigAttribute>().XmlLabel,
            //        IsRequired = x.GetCustomAttribute<PropertyConfigAttribute>().IsRequired,
            //        Value = x.GetValue(config).ToString(),
            //        IsList = (x.GetCustomAttribute<PropertyConfigAttribute>().ValueType == ValueCollectionType.List),
            //        Delimeter = x.GetCustomAttribute<PropertyConfigAttribute>().Delimeter
            //    }).ToList();
            ////.ToDictionary(key => 
            ////        key.GetCustomAttribute<PropertyConfigAttribute>().XmlLabel,
            ////        key => key.GetValue(config).ToString());
            //return View(settings);

            PropertyConfigurationModel pcm = new PropertyConfigurationModel
            {
                Delimeter = ',',
                IsList = true,
                IsRequired = true,
                Value = "192.168.1.133,192.168.1.183",
                ValueType = ValueCollectionType.Normal,
                XmlLabel = "dummy"
            };

            //PropertyConfigurationModel pcm1 = new PropertyConfigurationModel
            //{
            //    Delimeter = ',',
            //    IsList = true,
            //    IsRequired = true,
            //    Value = "192.168.1.183",
            //    ValueType = ValueCollectionType.Normal,
            //    XmlLabel = "dummy2"
            //};

            //PropertyConfigurationModel pcm2 = new PropertyConfigurationModel
            //{
            //    Delimeter = ',',
            //    IsList = true,
            //    IsRequired = true,
            //    Value = "192.168.1.183",
            //    ValueType = ValueCollectionType.Normal,
            //    XmlLabel = "dummy3"
            //};

            PropertyConfigurationModel[] pm_arr = new PropertyConfigurationModel[1];
            pm_arr[0] = pcm;
            //pm_arr[1] = pcm1;
            //pm_arr[2] = pcm2;

            return View(pm_arr);
        }
    }
}
