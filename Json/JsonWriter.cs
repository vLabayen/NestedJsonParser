using System;
using System.Text;
using System.Reflection;

public class JsonWriter {

  public static string WriteJson(object obj) {
    //Start the json object
    StringBuilder sb = new StringBuilder("{");

    bool prev = false; //Whether or not a previous value where added, so a "," is necesary
    foreach(PropertyInfo p in obj.GetType().GetProperties()) {
      if (prev == true) sb.Append(",");
      else prev = true;

      //Basic data types
      if (JsonWriter.IsBasicDataType(p.PropertyType)){
        sb.AppendFormat("\"{0}\":{1}", p.Name, JsonWriter.WriteBasicDataType(p.GetValue(obj), p.PropertyType));

      //Array type: Nested array not suported
      } else if (p.PropertyType.IsArray){
        //Create a new string builder for the content of the array
        StringBuilder array_sb = new StringBuilder();

        //Get type of the array and its values
        Type arrayType = p.PropertyType.GetElementType();
        if (arrayType.IsArray) throw new Exception("nested arrays not suported yet");
        Array genericArray = (Array) p.GetValue(obj);

        bool prev2 = false;
        for(int i = 0; i< genericArray.Length; i++){
          if (prev2 == true) array_sb.Append(",");
          else prev2 = true;

          //Basic data types
          if (JsonWriter.IsBasicDataType(arrayType)){
            array_sb.Append(JsonWriter.WriteBasicDataType(genericArray.GetValue(i), arrayType));

          //Class type --> recusion to obtain the json of each object
          } else if (arrayType.IsClass) {
            array_sb.Append(JsonWriter.WriteJson(genericArray.GetValue(i)));
          }
        }

        //Add the array name and its content
        sb.AppendFormat("\"{0}\":[{1}]", p.Name, array_sb.ToString());

      //Class type ---> recusion
      } else if (p.PropertyType.IsClass) {
        sb.AppendFormat("\"{0}\":{1}", p.Name, JsonWriter.WriteJson(p.GetValue(obj)));
      }
    }

    //Close the object and return the json
    sb.Append("}");
    return sb.ToString();
  }

  public static bool IsBasicDataType(Type type){
    return (
      (type == typeof(string)) ||
      (type == typeof(int)) ||
      (type == typeof(float)) ||
      (type == typeof(bool))
    );
  }

  public static string WriteBasicDataType(object value, Type type){
    if (type == typeof(string)) return JsonWriter.StringType(value);
    else if (type == typeof(int)) return JsonWriter.IntType(value);
    else if (type == typeof(float)) return JsonWriter.FloatType(value);
    else if (type == typeof(bool)) return JsonWriter.BoolType(value);
    else throw new Exception("Provided type is not a basic one");
  }

  public static string StringType(object value){
    return String.Format("\"{0}\"", value);
  }
  public static string IntType(object value){
    return ((int)value).ToString();
  }
  public static string FloatType(object value){
    return ((float)value).ToString();
  }
  public static string BoolType(object value){
    return ((bool)value == true) ? "true" : "false";
  }

}
