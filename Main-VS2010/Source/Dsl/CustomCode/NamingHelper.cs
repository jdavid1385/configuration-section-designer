using System;

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
    }
}