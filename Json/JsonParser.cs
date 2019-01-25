//https://dotnetfiddle.net/PvKRH0
//https://stackoverflow.com/questions/12294242/how-to-set-vaues-to-the-nested-property-using-c-sharp-reflection
//https://stackoverflow.com/questions/51934180/vb-net-instantiate-a-nested-property-by-reflection/51952086#51952086
//https://stackoverflow.com/questions/9783191/setting-value-in-an-array-via-reflection

using System;
using System.Reflection;
using System.Collections.Generic;

public class JsonParser{

    public static T FromJson<T>(string json) where T : new() {
      T obj = new T();

      object target = obj;
      JsonReader reader = new JsonReader(json);
      JsonParser.FillObject(ref target, reader.properties);

      return (T)target;
    }

    private static void FillObject(ref object target, List<JsonProperty> properties){
      foreach(JsonProperty jp in properties){
        PropertyInfo propertyToSet = target.GetType().GetProperty(jp.name);
        if ( !(jp.dataType == DataType.Data_Array) && !(jp.dataType == DataType.Data_Object) )
          propertyToSet.SetValue(target, jp.value);
        else if(jp.dataType == DataType.Data_Array){ //ALL ITEMS OF THE ARRAY MUST BE OF THE SAME DATATYPE
          List<JsonProperty> jsonArray = (List<JsonProperty>)jp.value;
          if (jsonArray.Count == 0) {
            propertyToSet.SetValue(target, Array.CreateInstance(propertyToSet.PropertyType.GetElementType(), 0));
            continue;
          }
          if (jsonArray[0].dataType == DataType.Data_Array) throw new Exception("nested arrays not suported");
          if (jsonArray[0].dataType == DataType.Data_Object){
            Array objects = (Array) propertyToSet.GetValue(target);
            if (objects == null) objects = Array.CreateInstance(propertyToSet.PropertyType.GetElementType(), jsonArray.Count);
            for (int i = 0; i < jsonArray.Count; i++){
              object property_value_arr  = Activator.CreateInstance(propertyToSet.PropertyType.GetElementType());
              FillObject(ref property_value_arr, (List<JsonProperty>) jsonArray[i].value);
              objects.SetValue(property_value_arr, i);
            }
            propertyToSet.SetValue(target, objects);
            continue;
          }
          if (jsonArray[0].dataType == DataType.Data_Int){
            int[] temp_arr_int = new int[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++) temp_arr_int[i] = (int)jsonArray[i].value;
            propertyToSet.SetValue(target, temp_arr_int);
          }else if(jsonArray[0].dataType == DataType.Data_Bool){
            bool[] temp_arr_bool = new bool[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++) temp_arr_bool[i] = (bool)jsonArray[i].value;
            propertyToSet.SetValue(target, temp_arr_bool);
          }else if (jsonArray[0].dataType == DataType.Data_String){
            string[] temp_arr_string = new string[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++) temp_arr_string[i] = (string)jsonArray[i].value;
            propertyToSet.SetValue(target, temp_arr_string, null);
          }
        }
        else {
          object property_value_obj = Activator.CreateInstance(propertyToSet.PropertyType);
          propertyToSet.SetValue(target, property_value_obj);
          List<JsonProperty> obj_properties = (List<JsonProperty>) jp.value;
          FillObject(ref property_value_obj, obj_properties);
        }
      }
    }

}
