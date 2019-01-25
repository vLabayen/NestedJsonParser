using System;
using System.Collections.Generic;

public enum DataType {
  Data_String, Data_Bool, Data_Int, Data_Array, Data_Object
}
public class JsonProperty{
  public DataType dataType;
  public string name;
  public object value;
}

public class JsonReader{
  public List<JsonProperty> properties = new List<JsonProperty>();

  public JsonReader(string json){
    string rest = AvanceTo(json, '{');

    while(rest[0] != '}'){
      JsonProperty data = new JsonProperty();

      //GET THE NAME
      SetPropertyName(ref rest, ref data);
      //SET THE VALUE
      SetPropertyValue(ref rest, ref data);
      //ADD THE PROPERTY
      this.properties.Add(data);
    }
  }

  private void SetPropertyName(ref string rest, ref JsonProperty data){
    rest = AvanceTo(rest, '"');
    data.name = rest.Substring(0, rest.IndexOf('"'));
    rest = AvanceTo(rest, '"');
    rest = AvanceTo(rest, ':');
  }

  private void SetPropertyValue(ref string rest, ref JsonProperty data){
    switch(rest[0]){
      case '"':
        ReadString(ref rest, ref data);
        break;
      case 't':
      case 'f':
        ReadBool(ref rest, ref data);
        break;
      case '[':
        ReadArray(ref rest, ref data);
        break;
      case '{':
        ReadObject(ref rest, ref data);
        break;
      default:
        if (Char.IsDigit(rest[0]) || (rest[0] == '-')){
          ReadInt(ref rest, ref data);
        }else throw new Exception("Not a json");
        break;
    }
    //AVANCE IF POSSIBLE
    if (rest[0] == ',') rest = AvanceTo(rest, ',');
  }

  private void ReadString(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_String;
    rest = AvanceTo(rest, '"');
    data.value = rest.Substring(0, rest.IndexOf('"'));
    rest = AvanceTo(rest, '"');
  }
  private void ReadBool(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Bool;
    data.value = (rest[0] == 't') ? true : false;
    rest = AvanceTo(rest, 'e');
  }
  private void ReadInt(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Int;
    int len = 1;
    while( !(rest[len] == ',') && !(rest[len] == ']') && !(rest[len] == '}') ) len++;
    data.value = Int32.Parse(rest.Substring(0, len) );
    rest = rest.Substring(len);
  }
  private void ReadArray(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Array;
    rest = AvanceTo(rest, '[');
    List<JsonProperty> temp = new List<JsonProperty>();

    while(rest[0] != ']'){
      JsonProperty arr_value = new JsonProperty();
      SetPropertyValue(ref rest, ref arr_value);
      temp.Add(arr_value);
    }

    data.value = temp;
    rest = AvanceTo(rest, ']');
  }
  private void ReadObject(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Object;
    rest = AvanceTo(rest, '{');
    List<JsonProperty> temp = new List<JsonProperty>();

    while(rest[0] != '}'){
      JsonProperty obj_prop = new JsonProperty();
      SetPropertyName(ref rest, ref obj_prop);
      SetPropertyValue(ref rest, ref obj_prop);
      temp.Add(obj_prop);
    }

    data.value = temp;
    rest = AvanceTo(rest, '}');
  }

  private string AvanceTo(string s, char c){
    int index = s.IndexOf(c);
    if (index == -1) throw new Exception("Not a json");
    return s.Substring(index + 1);
  }

}
