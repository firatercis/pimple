using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace sr
{
  /**
   * Provides a search field for PropSheets. PropSheet is a class used internally
   * for searching in materials. It is a generic type (for ex. PropSheet<float> or PropSheet<Texture>).
   */
  [System.Serializable]
  public class DynamicTypePropSheet : DynamicTypeData
  {

    [System.NonSerialized]
    InitializationContext initializationContext;

    DynamicTypeField typeField;

    public override void Draw(SearchOptions options)
    {
      Type subType = parent.type.GetGenericArguments()[0];
      initializationContext = new InitializationContext(subType);

      // Debug.Log("[DynamicTypeCollection] array of "+subType);
      if(typeField == null)
      {
        typeField = new DynamicTypeField();
        typeField.OnDeserialization();
      }
      typeField.SetType(initializationContext);
      typeField.Draw(options);
    }

    public override void OnDeserialization()
    {
      if(typeField != null)
      {
        typeField.OnDeserialization();
      }
    }

    public override void SearchProperty( SearchJob job, SearchItem item, SerializedProperty prop)
    {
      //Time for some freakish magic!
      SerializedProperty iterator = prop.Copy();
      while(iterator.NextVisible(true))
      {
        if(typeField.PropertyType() == iterator.propertyType)
        {
          //might need to add a guard against collections or something?
          typeField.SearchProperty(job, item, iterator);
        }
      }
      // iterator.Next(true);
      // iterator.Next(true);
      // int count = iterator.intValue;
      // for(int i=0;i < count; i++)
      // {
      //   iterator.Next(false);
      //   typeField.SearchProperty(job, item, iterator);
      // }
    }

    public override void ReplaceProperty(SearchJob job, SerializedProperty prop, SearchResult result)
    {
      typeField.ReplaceProperty(job, prop, result);
    }

    public override bool IsValid()
    {
      return typeField.IsValid();
    }

    public override string StringValue()
    {
      if(typeField == null)
      {
        return "";
      }
      return typeField.StringValue();
    }
  }
}
