using System;
using System.Globalization; //This namespace is used to use NumberStyles.Any and CultureInfo.InvariantCulture
//They are used while casting a float to ensure that the decimals numbers separator is a .
using System.Collections.Generic;

public enum DataType {
  Data_null, Data_String, Data_Bool, Data_Int, Data_Float, Data_Array, Data_Object
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
        bool posibleInt = true;
        bool posibleFloat = false;
        //Check if the value can be casted to an int or float
        //If not, it will be considered as a string
        int i = 1;
        while(rest[i] != '"') { //For each character in the data
          if(!Char.IsDigit(rest[i])){ //If the character is not a digit, it could be a '-', '.' or ','
            //If is a dot or a comma, we cannot cast the value to an int anymore and we should consider it as a float
            //But as a float, it can only have one dot or , in all the data ¿¿¿TODO???: SUPPORT SOMETHING LIKE 10.000,24???
            //https://docs.microsoft.com/en-us/dotnet/api/system.single.tryparse?view=netframework-4.7.2#System_Single_TryParse_System_String_System_Globalization_NumberStyles_System_IFormatProvider_System_Single__
            if ((rest[i] == '.' || rest[i] == ',') && (posibleFloat == false)) {
              posibleInt = false;
              posibleFloat = true;
            }else
            //If is the minus, it can only be an int/float if the - is the first character
            //Any other case, the data cannot be neither an int or a float
            if (!((i == 1) && (rest[i] == '-'))) {
              posibleInt = false;
              posibleFloat = false;
              break;
            }
          }
          i++;
        }
        if (posibleInt) ReadIntInQuotation(ref rest, ref data);
        else if (posibleFloat) ReadFloatInQuotation(ref rest, ref data);
        else ReadString(ref rest, ref data);
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
          bool canBeFloat = false;
          int len = 1;
          //Check the whole number to check how many dots it have.
          //a ',' represents the next property or item in a array, a ']' or '}' represents the end of the arr/obj
          //No dots results in that the data is considered as an int
          //One dot results in that the data is considered as a float
          //More dots results in a Exception
          while( !(rest[len] == ',') && !(rest[len] == ']') && !(rest[len] == '}') ){
            if (rest[len] == '.'){
              if (canBeFloat == false) canBeFloat = true;
              else throw new Exception(String.Format("Second dot found in {0} of {1} property while reading it as float", rest, data.name));
            }else if (!Char.IsDigit(rest[len])) throw new Exception(String.Format("Invalid character found in {0} of {1} property while reading as int/float", rest, data.name));

            len++;
          }
          if (canBeFloat == false) ReadInt(ref rest, ref data);
          else ReadFloat(ref rest, ref data);
        }else throw new Exception(String.Format("No valid type found for {0} property", data.name));
        break;
    }
    //AVANCE IF POSSIBLE
    if (rest[0] == ',') rest = AvanceTo(rest, ',');
  }

  //READ ARRAY AND READ OBJECT CAN BE RECUSIVE IF IN SET PROPERTY THEY ARE CALLED AGAIN
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

  //"SAFE" READING FUNCTIONS
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
  //UNSAFE READING FUNCTIONS
  private void ReadIntInQuotation(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Int;
    rest = AvanceTo(rest, '"');
    int value;
    if(Int32.TryParse(rest.Substring(0, rest.IndexOf('"')), out value)) data.value = value;
    else throw new Exception(String.Format(
      "Cound't parse {0} of property {1} (type int with \"\")", rest.Substring(0, rest.IndexOf('"')), data.name
    ));
    rest = AvanceTo(rest, '"');
  }
  private void ReadFloatInQuotation(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Float;
    rest = AvanceTo(rest, '"');
    float value;
    //We have to make a replace of , for . because if the string were into "" a ',' doesnt represent the next element, so floats can be expresed as "10.2" or "10,2"
    if(float.TryParse(rest.Substring(0, rest.IndexOf('"')).Replace(',','.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value)) data.value = value;
    else throw new Exception(String.Format(
      "Cound't parse {0} of property {1} (type float)", rest.Substring(0, rest.IndexOf('"')), data.name
    ));
    rest = AvanceTo(rest, '"');
  }
  private void ReadInt(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Int;
    int len = 1;
    while( !(rest[len] == ',') && !(rest[len] == ']') && !(rest[len] == '}') ) len++;
    int value;
    if(Int32.TryParse(rest.Substring(0, len), out value)) data.value = value;
    else throw new Exception(String.Format(
      "Cound't parse {0} of property {1} (type int)", rest.Substring(0, len), data.name
    ));
    rest = rest.Substring(len);
  }
  private void ReadFloat(ref string rest, ref JsonProperty data){
    data.dataType = DataType.Data_Float;
    int len = 1;
    while( !(rest[len] == ',') && !(rest[len] == ']') && !(rest[len] == '}') ) len++;
    float value;
    if(float.TryParse(rest.Substring(0, len), NumberStyles.Any, CultureInfo.InvariantCulture, out value)) data.value = value;
    else throw new Exception(String.Format(
      "Cound't parse {0} of property {1} (type float)", rest.Substring(0, len), data.name
    ));
    rest = rest.Substring(len);
  }


  //This function cuts the string at the first found 'c' character. c is not returned. If the character couldn't be found a Exception if thrown
  private string AvanceTo(string s, char c){
    int index = s.IndexOf(c);
    if (index == -1) throw new Exception(String.Format("Unexpected en of json while searching for {0}", c));
    return s.Substring(index + 1);
  }

}
