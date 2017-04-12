using System;
using System.Text.RegularExpressions;

namespace ConfigurationSectionDesigner
{
    /// <summary>
    /// Provides helper methods for naming elements and properties.
    /// </summary>
    internal static class NamingHelper
    {
        /// <summary>
        /// Converts an identifier to camelCase.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>The given identifier with the first character converted to lower case.</returns>
        public static string ToCamelCase(string identifier)
        {
            string newIdentifier = identifier;
            if (!string.IsNullOrEmpty(newIdentifier))
            {
                newIdentifier = newIdentifier.Substring(0, 1).ToLower() + newIdentifier.Substring(1);
            }
            return newIdentifier;
        }

        /// <summary>
        /// Checks whether name is a valid variable name. If empty, no check performed.
        /// [20121217] Changing to simple SPACE check for now (most common validation issue). Was causing some issues and I do not have time to resolve yet.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidName(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.IndexOf(' ') >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            // MATCH:       myName, my0Name, myName0, MyName`
            // NOT MATCH:   0myName, my Name
            //Regex re = new Regex("^[^0-9\\.][a-zA-Z0-9\\.]+[\\`]?$");
            //return (re.IsMatch(name));
        }

        /// <summary>
        /// Checks whether name is a valid variable name. If empty, no check performed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidOrEmptyName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            else
            {
                return (IsValidName(name));
            }
        }

        /// <summary>
        /// Checks whether name is a valid xml name.
        /// [20121217] Changing to simple SPACE check for now (most common validation issue). Was causing some issues and I do not have time to resolve yet.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidXmlName(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.IndexOf(' ') >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            // MATCH:       myName, my0Name, myName0, MyName`
            // NOT MATCH:   0myName, my Name
            //Regex re = new Regex("^[^0-9\\.][a-zA-Z0-9\\.\\:]+[\\`]?$");
            //return (re.IsMatch(name));
        }

        public static bool IsValidOrEmptyXmlName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            else
            {
                return (IsValidXmlName(name));
            }
        }
        
    }
}