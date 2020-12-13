using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Civil3d.CatalogTools
{
    public static class PipeCatalogServices
    {
        private static readonly Dictionary<string, string[]>
            _famParameters = new Dictionary<string, string[]>();
        
        public static bool CatalogContainsThisFamilyGuid(string guidString, DomainType type)
        {
            if (guidString is null)
            {
                return false;
            }

            Guid guid = new Guid(guidString);
            DataPartFamily[] famsData = PartsList.GetAvailablePartFamilies(type);
            return famsData.Any(data => new Guid(data.GUID) == guid);
        }

        public static string[] GetCatalogParameterNames(string familyGuid)
        {
            if (_famParameters.TryGetValue(familyGuid, out string[] storedParameters))
            {
                return storedParameters;
            }

            // Читаем путь к каталогу
            string catalogPath = GetConnectedCatalogDirectoryPath();

            // Получаем из каталога данные об APC файлах
            string[] apcFilesFromCatalog = GetAllApcFiles(catalogPath);

            string[] parameters = null;

            // Получаем из APC файлов данные о семействах
            foreach (string apcFilePath in apcFilesFromCatalog)
            {
                XElement apcFileAsXml = SafeLoad(apcFilePath);
                XElement part = apcFileAsXml?.Descendants("Part")
                    .FirstOrDefault(item => item.Attribute("id")?.Value
                    .Equals(familyGuid, StringComparison.OrdinalIgnoreCase) ?? false);
                if (part != null)
                {
                    string relPathToXml = part
                        .Element("TableRef")?
                        .Element("URL")?
                        .Attributes()?
                        .FirstOrDefault()?
                        .Value;
                    if (!string.IsNullOrEmpty(relPathToXml))
                    {
                        string pathToXml = Path.Combine
                            (Path.GetDirectoryName(apcFilePath), relPathToXml);
                        XElement xml = SafeLoad(pathToXml);
                        parameters = xml?.Elements()?
                            .Select(item => item.Attribute("name")?.Value)
                            .Where(item => !string.IsNullOrEmpty(item))
                            .ToArray();
                        break;
                    }
                }
            }

            if (parameters != null)
            {
                _famParameters[familyGuid] = parameters;
            }

            return parameters;
        }

        public static void ClearCachedData()
        {
            _famParameters.Clear();
        }
        
        private static string[] GetAllApcFiles(string catalogDirPath)
        {
            List<string> apcFilePaths = new List<string>();

            if (Directory.Exists(catalogDirPath))
            {
                string[] dirs = Directory.GetDirectories(catalogDirPath);

                foreach (string dir in dirs)
                {
                    string apcFilePath
                        = Path.Combine(dir, Path.GetFileName(dir) + ".apc");

                    if (File.Exists(apcFilePath))
                    {
                        apcFilePaths.Add(apcFilePath);
                    }
                }
            }
            return apcFilePaths.ToArray();
        }

        private static string GetConnectedCatalogDirectoryPath()
        {
            RegistryKey netKey = GetNetworkRegistryKey();
            string sharContPath;
            using (netKey)
            {
                try
                {
                    sharContPath = (string)netKey?.GetValue("SharedContentPath");
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    sharContPath = null;
                }
            }

            string catPath = null;
            if (!string.IsNullOrEmpty(sharContPath)
                && Directory.Exists(sharContPath))
            {
                try
                {
                    catPath = Directory
                        .GetParent(sharContPath)
                        .FullName;
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }

            return catPath;
        }

        private static RegistryKey GetNetworkRegistryKey()
        {
            string productKeyName
                    = HostApplicationServices.Current.UserRegistryProductRootKey;
            string profileName
                = Application.GetSystemVariable("CPROFILE").ToString();

            RegistryKey genKey;
            try
            {
                genKey = Registry.CurrentUser.OpenSubKey
                    ($@"{productKeyName}\Profiles\{profileName}\Preferences", false);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                genKey = null;
            }

            RegistryKey netKey = null;
            using (genKey)
            {
                try
                {
                    string netSubKeyName = genKey?
                        .GetSubKeyNames()
                        .FirstOrDefault(item => item.Contains("AeccUiNetwork"));

                    if (!string.IsNullOrEmpty(netSubKeyName))
                    {
                        netKey = genKey?.OpenSubKey(netSubKeyName, false);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    netKey = null;
                }
            }

            return netKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static XElement SafeLoad(string path)
        {
            try
            {
                return XElement.Load(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);                
                return null;
            }
        }
    }
}
