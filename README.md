# Unity_FromJsonRecusiveParser
2 classes that allow to cast a json to any provided generic type.

Usage example in Test and TestObjects class.
All code in JsonParser and JsonReader.

Supported types :
  - Int
  - Bool
  - String
  - Arrays of int,bool,string
  - Classes
  - Arrays of custom classes

Not supported types:
  - Floats and double
  - Nested arrays

JSON Syntax notes : 
  - No spaces allowed out of a string
      -> {"name" : "vic"} will throw an error.
      -> {"name":"victor labayen"} shouldn't complain
  - Booleans as {"success":true} instead of {"success":"true"}
  - Integeres as {"myint":5} instead of {"myint":"5"}
    - Inside a class {} every field must have a name -> {"field1":true}
    - Inside an array the fields must not have a name -> {"myarray":[true,true,true]}
