using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace ConfigurationSectionDesigner
{
    public partial class AttributeProperty
    {
        #region Convenience Properties

        /// <summary>
        /// Gets the type name of this property.
        /// </summary>
        public override string TypeName
        {
            get
            {
                // Return the type name of the TypeDefinition that is set as this Attribute's type.
                return string.Format("{0}.{1}", this.Type.Namespace, this.Type.Name);
            }
        }

        #endregion
    }
}