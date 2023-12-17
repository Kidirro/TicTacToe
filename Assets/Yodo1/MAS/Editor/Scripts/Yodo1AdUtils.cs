namespace Yodo1.MAS
{
    using UnityEditor;
    using System.IO;
    using System.Xml;
    using System;
    using UnityEngine;

    public class Yodo1AdUtils
    {
        /// <summary>
        /// Show Alert
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="positiveButton"></param>
        public static void ShowAlert(string title, string message, string positiveButton)
        {
            if (!string.IsNullOrEmpty(positiveButton))
            {
                int index = EditorUtility.DisplayDialogComplex(title, message, positiveButton, "", "");

            }
            return;
        }

        private static readonly string VERSION_PATH = Path.GetFullPath(".") + "/Assets/Yodo1/MAS/version.xml";

        public static string GetPluginVersion()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(VERSION_PATH, settings);

            XmlDocument xmlReadDoc = new XmlDocument();
            xmlReadDoc.Load(VERSION_PATH);
            XmlNode xnRead = xmlReadDoc.SelectSingleNode("versions");
            XmlElement unityNode = (XmlElement)xnRead.SelectSingleNode("unity");
            string version = unityNode.GetAttribute("version").ToString();
       
            reader.Close();
            return version;
        }


        private static readonly string ANDROID_DEPENDENCIES_PATH = Path.GetFullPath(".") + "/Assets/Yodo1/MAS/Editor/Dependencies/Yodo1MasAndroidDependencies.xml";

        public static bool IsMASCN()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(ANDROID_DEPENDENCIES_PATH, settings);

            XmlDocument xmlReadDoc = new XmlDocument();
            xmlReadDoc.Load(ANDROID_DEPENDENCIES_PATH);
            XmlNode dependenciesNode = xmlReadDoc.SelectSingleNode("dependencies");
            XmlNode packagesNode = dependenciesNode.SelectSingleNode("androidPackages");
            XmlElement androidPackageNode = (XmlElement)packagesNode.SelectSingleNode("androidPackage");
            string specString = androidPackageNode.GetAttribute("spec").ToString();

            //Debug.Log(Yodo1U3dMas.TAG + "IsMASCN method: specString: " + specString);

            reader.Close();

            if(specString == null)
            {
                return false;
            }

            if(specString.Contains(":"))
            {
                string[] splitArray = specString.Split(new Char[] { ':' });
                {
                    string model = splitArray[1];
                    //Debug.Log(Yodo1U3dMas.TAG + "IsMASCN method: model: " + model);
                    if (!string.IsNullOrEmpty(model) && model.Equals("cn"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }


}