using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace ConfigurationSectionDesigner
{
    /// <summary>
    /// Used on the custom attributes ConfigurationProperty in order to
    /// provide a user interface for editing custom attributes.
    /// </summary>
    public class AttributeEditor : UITypeEditor
    {
        [SecurityPermission( SecurityAction.LinkDemand )]
        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            if( context == null )
            {
                throw new ArgumentNullException( "context" );
            }
            if( provider == null )
            {
                throw new ArgumentNullException( "provider" );
            }
            if( value == null )
            {
                throw new ArgumentNullException( "value" );
            }

            ElementPropertyDescriptor propertyDescriptor = context.PropertyDescriptor as ElementPropertyDescriptor;
            if( (propertyDescriptor != null) && ((propertyDescriptor.ModelElement is ConfigurationProperty)) )
            {
                ConfigurationProperty configElement = propertyDescriptor.ModelElement as ConfigurationProperty;
                LinkedElementCollection<Attribute> attributes = configElement.Attributes;

                using (Transaction transaction = configElement.Store.TransactionManager.BeginTransaction("Edit custom attributes"))
                {
                    using( AttributesForm attributeForm = new AttributesForm( configElement.Store, attributes ) )
                    {
                        if( attributeForm.ShowDialog() == DialogResult.OK && transaction.HasPendingChanges )
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                }
            }

            return value;
        }

        [SecurityPermission( SecurityAction.LinkDemand )]
        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }

    }

}
