using Autodesk.Civil.DatabaseServices;
using System.Linq;

namespace Civil3d.CatalogTools
{
    public static class PartDataRecordExtensions
    {
        public static PartDataField[] GetAllActualDataFields(this PartDataRecord partDataRec, string partFamilyGuid)
        {
            string[] paramNames = PipeCatalogServices.GetCatalogParameterNames(partFamilyGuid);

            PartDataField[] dataFields = partDataRec.GetAllDataFields();
            if (paramNames != null)
            {
                dataFields = dataFields
                    .Where(dataField => paramNames.Contains(dataField.Name))
                    .ToArray();
            }

            return dataFields;
        }
    }
}
