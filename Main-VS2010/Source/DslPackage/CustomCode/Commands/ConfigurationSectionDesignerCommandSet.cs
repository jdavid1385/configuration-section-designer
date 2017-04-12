using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using System.Windows.Forms;
using System.Reflection;

namespace ConfigurationSectionDesigner
{
    internal partial class ConfigurationSectionDesignerCommandSet
    {
        #region Private Methods

        private void MovePropertyUp()
        {
            foreach( object selectedObject in this.CurrentSelection )
            {
                AttributeProperty attribute = selectedObject as AttributeProperty;
                if( attribute != null )
                {
                    LinkedElementCollection<AttributeProperty> attributes = attribute.ConfigurationElement.AttributeProperties;
                    int curIndex = attributes.IndexOf( attribute );
                    if( curIndex == 0 )
                        return;
                }
                ElementProperty element = selectedObject as ElementProperty;
                if( element != null )
                {
                    LinkedElementCollection<ElementProperty> elements = element.ConfigurationElement.ElementProperties;
                    int curIndex = elements.IndexOf( element );
                    if( curIndex == 0 )
                        return;
                }
            }

            using( Transaction transaction = ((ModelElement)this.SingleDocumentSelection).Store.TransactionManager.BeginTransaction() )
            {
                try
                {
                    foreach( object selectedObject in this.CurrentSelection )
                    {
                        AttributeProperty attribute = selectedObject as AttributeProperty;
                        if( attribute != null )
                        {
                            LinkedElementCollection<AttributeProperty> attributes = attribute.ConfigurationElement.AttributeProperties;
                            int curIndex = attributes.IndexOf( attribute );

                            attributes.Move( attribute, curIndex - 1 );
                        }
                        ElementProperty element = selectedObject as ElementProperty;
                        if( element != null )
                        {
                            LinkedElementCollection<ElementProperty> elements = element.ConfigurationElement.ElementProperties;
                            int curIndex = elements.IndexOf( element );

                            elements.Move( element, curIndex - 1 );
                        }
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                transaction.Commit();
            }
        }

        private void MovePropertyDown()
        {
            foreach( object selectedObject in this.CurrentSelection )
            {
                AttributeProperty attribute = selectedObject as AttributeProperty;
                if( attribute != null )
                {
                    LinkedElementCollection<AttributeProperty> attributes = attribute.ConfigurationElement.AttributeProperties;
                    int curIndex = attributes.IndexOf( attribute );
                    if( curIndex == attributes.Count - 1 )
                        return;
                }
                ElementProperty element = selectedObject as ElementProperty;
                if( element != null )
                {
                    LinkedElementCollection<ElementProperty> elements = element.ConfigurationElement.ElementProperties;
                    int curIndex = elements.IndexOf( element );
                    if( curIndex == elements.Count - 1 )
                        return;
                }
            }

            using( Transaction transaction = ((ModelElement)this.SingleDocumentSelection).Store.TransactionManager.BeginTransaction() )
            {
                try
                {
                    foreach( object selectedObject in this.CurrentSelection )
                    {
                        AttributeProperty attribute = selectedObject as AttributeProperty;
                        if( attribute != null )
                        {
                            LinkedElementCollection<AttributeProperty> attributes = attribute.ConfigurationElement.AttributeProperties;
                            int curIndex = attributes.IndexOf( attribute );

                            attributes.Move( attribute, curIndex + 1 );
                        }
                        ElementProperty element = selectedObject as ElementProperty;
                        if( element != null )
                        {
                            LinkedElementCollection<ElementProperty> elements = element.ConfigurationElement.ElementProperties;
                            int curIndex = elements.IndexOf( element );

                            elements.Move( element, curIndex + 1 );
                        }
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                transaction.Commit();
            }
        }

        private void ImportExternalEnum()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "dll",
                Filter = "Assemblies (*.dll)|*.dll|All Files (*.*)|*.*"
            };
            if( ofd.ShowDialog() == DialogResult.OK )
            {
                string file = ofd.FileName;
                Assembly assembly = Assembly.LoadFrom( file );

                List<Type> enumTypes = new List<Type>();
                foreach( Type type in assembly.GetExportedTypes() )
                {
                    if( type.IsEnum )
                    {
                        enumTypes.Add( type );
                    }
                }

                if( enumTypes.Count == 0 )
                {
                    MessageBox.Show( "The chosen assembly has no public Enums.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                ConfigurationSectionDesignerDiagram diagram = this.SingleDocumentSelection as ConfigurationSectionDesignerDiagram;
                using( Transaction transaction = diagram.Store.TransactionManager.BeginTransaction( "Import External Enum" ) )
                {
                    using( ImportEnumForm importEnumForm = new ImportEnumForm( enumTypes ) )
                    {
                        if( importEnumForm.ShowDialog() == DialogResult.OK )
                        {
                            ConfigurationSectionModel model = diagram.ModelElement as ConfigurationSectionModel;

                            Type enumType = importEnumForm.SelectedEnum;

                            FlagsAttribute fa = enumType.GetCustomAttributes( false )
                                .Where( a => a is FlagsAttribute )
                                .SingleOrDefault() as FlagsAttribute;

                            EnumeratedType enumeratedType = new EnumeratedType( diagram.Store );
                            enumeratedType.CodeGenOptions = TypeDefinitionCodeGenOptions.None;
                            enumeratedType.Name = enumType.Name;
                            enumeratedType.Namespace = enumType.Namespace;
                            enumeratedType.IsFlags = fa != null;

                            FieldInfo[] fields = enumType.GetFields( BindingFlags.Public | BindingFlags.Static );
                            foreach( FieldInfo field in fields )
                            {
                                EnumerationLiteral enumeratedLiteral = new EnumerationLiteral( diagram.Store );
                                enumeratedLiteral.Name = field.Name;

                                enumeratedType.Literals.Add( enumeratedLiteral );
                            }

                            model.TypeDefinitions.Add( enumeratedType );
                            transaction.Commit();
                        }
                        else
                            transaction.Rollback();
                    }
                }
            }
        }

        #endregion
    }
}
