using Autodesk.Civil.DatabaseServices;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Civil3d.CatalogTools
{
    public static class PartExtensions
    {
        static MethodInfo _method;

        static MethodInfo GetMethod()
        {
            if (_method is null)
            {
                Type partType = typeof(Part);
                MethodInfo[] methods = partType.GetMethods
                    (BindingFlags.NonPublic | BindingFlags.Instance);
                _method = methods.Single
                    (item => item.Name.Equals("InnerGetPropertyStringValue",
                    StringComparison.OrdinalIgnoreCase)
                    && item.GetParameters()[0].ParameterType.Equals(typeof(uint)));
            }
            return _method;
        }

        /// <summary>
        /// Метод получения значения строкового параметра для элемента сети
        /// </summary>
        /// <param name="part">Элемент сети, открытый на чтение</param>
        /// <param name="type">Тип параметра - перечисление</param>
        /// <returns>
        /// Строковое значение параметра или *ERROR*,
        /// если такой параметр не задан у элемента
        /// </returns>
        /// <example>
        /// string familyName = GetPartPropertyString
        ///     (part, PartPropertyStringType.CatalogFamilyName);
        /// </example>
        public static string GetPartPropertyString
            (this Part part, PartPropertyStringType type)
        {
            uint id = (uint)type;

            if (part.PartType != PartType.UndefinedPartType)
            {
                try
                {
                    return (string)GetMethod()?.Invoke(part, new object[] { id });
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            return null;
        }

        /// <summary>
        /// Получение названия семейства для элемента сети
        /// </summary>
        /// <param name="part">Элемент сети</param>
        /// <returns></returns>        
        public static string GetPartFamilyName(this Part part)
            => part.GetPartPropertyString
                (PartPropertyStringType.CatalogFamilyDescription);

        /// <summary>
        /// Получение GUID семейства элементов
        /// </summary>
        /// <param name="part">Элемент</param>
        /// <returns>GUID в виде строки или null</returns>        
        public static string GetPartFamilyGuid(this Part part)
            => part.GetPartPropertyString(PartPropertyStringType.FamilyGuid);
    }
}
