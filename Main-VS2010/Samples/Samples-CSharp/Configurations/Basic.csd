<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="a0465866-2961-4b4b-b31c-dd4529609611" namespace="Samples.Configurations" xmlSchemaNamespace="urn:Samples.Configurations" assemblyName="Samples.Configurations" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="SimpleSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="simpleSection">
      <elementProperties>
        <elementProperty name="Simple" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="simple" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/a0465866-2961-4b4b-b31c-dd4529609611/SimpleElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="SimpleElement">
      <attributeProperties>
        <attributeProperty name="Foo" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="foo" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a0465866-2961-4b4b-b31c-dd4529609611/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Bar" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="bar" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/a0465866-2961-4b4b-b31c-dd4529609611/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>