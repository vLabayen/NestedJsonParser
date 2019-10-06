//https://dotnetfiddle.net/PvKRH0
//https://stackoverflow.com/questions/12294242/how-to-set-vaues-to-the-nested-property-using-c-sharp-reflection
//https://stackoverflow.com/questions/51934180/vb-net-instantiate-a-nested-property-by-reflection/51952086#51952086
//https://stackoverflow.com/questions/9783191/setting-value-in-an-array-via-reflection
using System;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

public class JsonParser{

    public static T FromJson<T>(string json) where T : new() {
      T obj = new T();

      object target = obj;
      JsonReader reader = new JsonReader(json);
      JsonParser.FillObject(ref target, reader.properties);

      return (T)target;
    }

    public static string ToJson<T>(T obj) where T : class {
      //Set float decimal separator to . to avoid problems
      //[12.2f, 2f, 3f] ---> [12.2, 2, 3] instead of [12,2, 2, 3]
      System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
      customCulture.NumberFormat.NumberDecimalSeparator = ".";
      System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

      return JsonWriter.WriteJson(obj);
    }

    private static void FillObject(ref object target, List<JsonProperty> properties){
      foreach(JsonProperty jp in properties){

        //Try to get the property of the object based on the readed json
        PropertyInfo propertyToSet = target.GetType().GetProperty(jp.name);
        if (propertyToSet == null) throw new Exception(jp.name+" property not find in the object template");

        //************BASIC DATATYPES********************************
        if ( !(jp.dataType == DataType.Data_Array) && !(jp.dataType == DataType.Data_Object) )
          //try to set the property. A Exception is thrown if the datatype do not match
          try { propertyToSet.SetValue(target, jp.value);
          } catch(Exception){ throw new Exception(String.Format(
              "a {0} type was read and the template have a diferent data type. The value readed was {1}",
              Enum.GetName(typeof(DataType), jp.dataType),
              jp.value
            ));
          }
        //************ARRAY DATATYPE********************************
        else if(jp.dataType == DataType.Data_Array){
          List<JsonProperty> jsonArray = (List<JsonProperty>)jp.value;

          //Check if the array is empty. Set a empty array if so
          if (jsonArray.Count == 0) {
            propertyToSet.SetValue(target, Array.CreateInstance(propertyToSet.PropertyType.GetElementType(), 0));
            continue;
          }

          //ALL ITEMS OF THE ARRAY MUST BE OF THE SAME DATATYPE
          //check for the above constrain. C# forces this while the reader doesnt.
          //We can have a float arr where some items were readed as ints
          //we can have a string arr where some items were readed as ints or floats
          DataType typ = DataType.Data_null;
          for(int i = 0; i < jsonArray.Count; i++){
            if (i == 0) typ = jsonArray[i].dataType;
            else if (typ != jsonArray[i].dataType) {
              //if we were having an int array, but we found a float, all the array can be casted to a float arr
              //so we change the suposed type of the array to float.
              if (typ == DataType.Data_Int && jsonArray[i].dataType == DataType.Data_Float) typ = DataType.Data_Float;
              //If we were having an int array or float arr, and a string is found, same for string arr
              else if ((typ == DataType.Data_Int || typ == DataType.Data_Float) && jsonArray[i].dataType == DataType.Data_String) typ = DataType.Data_String;

              //if we were having a float array and we found an int, the array can still be casted to a float arr
              //so we continue to the next item of the array
              else if (typ == DataType.Data_Float && jsonArray[i].dataType == DataType.Data_Int) continue;
              //same if we had a string arr and a int or float is found
              else if (typ == DataType.Data_String && (jsonArray[i].dataType == DataType.Data_Int) || jsonArray[i].dataType == DataType.Data_Float) continue;
              //Other cases, the arr cannot be casted correctly
              else throw new Exception(String.Format(
                "All the items must be of the same datatype. Cound't parse {0} to a {1} array",
                Enum.GetName(typeof(DataType), jsonArray[i].dataType),
                Enum.GetName(typeof(DataType), jp.dataType)
              ));
            }
          }

          //Parse the readed data in the json structure to the object template
          //Nested arrays
          if (typ == DataType.Data_Array) throw new Exception("nested arrays not suported yet");

          //Array of objects. Here we just create an empty for each item
          //and use this function to fill it before we set its value to the template
          if (typ == DataType.Data_Object){
            //empty array of objects
            Array objects = (Array) propertyToSet.GetValue(target);
            if (objects == null) objects = Array.CreateInstance(propertyToSet.PropertyType.GetElementType(), jsonArray.Count);
            //Fill each object of the array recusively
            for (int i = 0; i < jsonArray.Count; i++){
              object property_value_arr  = Activator.CreateInstance(propertyToSet.PropertyType.GetElementType());
              FillObject(ref property_value_arr, (List<JsonProperty>) jsonArray[i].value);
              objects.SetValue(property_value_arr, i);
            }
            //Set the array value
            propertyToSet.SetValue(target, objects);
            continue;
          }

          //Basic datatype arrays
          //we check for strings-floats-ints-bools, the order is important in the first 3 ones
          //because we could parse ints to floats or string and floats to strings

          //we cast to string if a string type was readed OR if an int/float type were readed, but the template has a string type
          if (typ == DataType.Data_String || ( (typ == DataType.Data_Int || typ == DataType.Data_Float) && (propertyToSet.PropertyType.GetElementType() == typeof(string)) ) ){ //STRING
            string[] temp_arr_string = new string[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++)
              //We can have some items of the array that were read as ints or floats. If so, we need to make a diferent cast
              if (jsonArray[i].dataType == DataType.Data_String) temp_arr_string[i] = (string)jsonArray[i].value;
              else if (jsonArray[i].dataType == DataType.Data_Int) temp_arr_string[i] = ((int)jsonArray[i].value).ToString();
              else temp_arr_string[i] = ((float)jsonArray[i].value).ToString();
            //Try to set the value, a exception is thrown if the object template do not match the array type
            try { propertyToSet.SetValue(target, temp_arr_string, null);
            } catch(ArgumentException){ throw new Exception(String.Format(
              "Couldn't parse string array to {0} in {1} property. Your template should change its type to fit the data",
              propertyToSet.PropertyType,
              jp.name
            ));}
          //we cast to float if the readed type was float or if the readed type was int but the template has a float type
          }else if(typ == DataType.Data_Float || (typ == DataType.Data_Int && (propertyToSet.PropertyType.GetElementType() == typeof(float))) ){  //FLOAT
            float[] temp_arr_float = new float[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++)
              //We can have some items of the array that were read as ints. If so, we need to make a diferent cast
              if (jsonArray[i].dataType == DataType.Data_Float) temp_arr_float[i] = (float)jsonArray[i].value;
              else temp_arr_float[i] = (float)(int)jsonArray[i].value;
            //Try to set the value, a exception is thrown if the object template do not match the array type
            try { propertyToSet.SetValue(target, temp_arr_float);
            } catch(ArgumentException){ throw new Exception(String.Format(
              "Couldn't parse float array to {0} in {1} property. Your template should change its type to fit the data",
              propertyToSet.PropertyType,
              jp.name
            ));}

          //we cast to int if the readed type is int and the template type is also int
          }else if(typ == DataType.Data_Int){  //INT
            int[] temp_arr_int = new int[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++) temp_arr_int[i] = (int)jsonArray[i].value;
            //Try to set the value, a exception is thrown if the object template do not match the array type
            try { propertyToSet.SetValue(target, temp_arr_int);
            } catch(ArgumentException){ throw new Exception(String.Format(
              "Couldn't parse int array to {0} in {1} property. Your template should change its type to fit the data",
              propertyToSet.PropertyType,
              jp.name
            ));}

          }else if(typ == DataType.Data_Bool){  //BOOL
            bool[] temp_arr_bool = new bool[jsonArray.Count];
            for(int i = 0; i < jsonArray.Count; i++) temp_arr_bool[i] = (bool)jsonArray[i].value;
            //Try to set the value, a exception is thrown if the object template do not match the array type
            try { propertyToSet.SetValue(target, temp_arr_bool);
            } catch(ArgumentException){ throw new Exception(String.Format(
              "Couldn't parse bool array to {0} in {1} property. Your template should change its type to fit the data ",
              propertyToSet.PropertyType,
              jp.name
            ));}

          }else throw new Exception(String.Format("array type not recogniced for property {0}", jp.name));
        }
        //************OBJECT DATATYPE********************************
        else {
          //Create an empty object and fill it recusively with this function
          object property_value_obj = Activator.CreateInstance(propertyToSet.PropertyType);
          propertyToSet.SetValue(target, property_value_obj);
          List<JsonProperty> obj_properties = (List<JsonProperty>) jp.value;
          FillObject(ref property_value_obj, obj_properties);
        }
      }
    }

}
