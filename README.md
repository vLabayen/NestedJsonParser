# NestedJsonParser
2 classes that allow to cast a json to any provided generic type.  
The parser and the reader are not Unity dependent while the tests are.  
The parser is implemented using reflection. Both are recusive aproaches  

To import it, just download JsonReader & JsonParser to your proyect folder  
Usage example in Test and TestObjects class.  


One usage example (extracted from the tests) : 

Given the following structure :


`public class TestObject7{`  
&nbsp;&nbsp;&nbsp;&nbsp;`public TestObject6[] test_arr { get; set; }  `  
`}`
`public class TestObject6{`  
&nbsp;&nbsp;&nbsp;&nbsp;`public NestedObject2 nested { get; set; }`  
`}`  
`public class NestedObject2 {`  
&nbsp;&nbsp;&nbsp;&nbsp;`public NestedObject[] nested_arr { get; set; }`  
&nbsp;&nbsp;&nbsp;&nbsp;public TestObject5 nested_test { get; set; }  
}  
public class TestObject5{  
&nbsp;&nbsp;&nbsp;&nbsp;public NestedObject[] nesteds { get; set; }  
}  
public class NestedObject {  
&nbsp;&nbsp;&nbsp;&nbsp;public string name { get; set; }  
&nbsp;&nbsp;&nbsp;&nbsp;public int id { get; set; }  
}  

And given a json string like :  

{"test_arr":[{  
&nbsp;&nbsp;&nbsp;&nbsp;nested:{  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nested_arr:[  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:vic,id:0},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:yuan,id:1000}  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nested_test:{  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nesteds:[  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:vic,id:0},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:yuan,id:1000},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:pepe,id:-30}  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}  
&nbsp;&nbsp;&nbsp;&nbsp;}  
},{  
&nbsp;&nbsp;&nbsp;&nbsp;nested:{  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nested_arr:[  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:vic,id:0},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:yuan,id:1000}  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nested_test:{  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;nesteds:[  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:vic,id:0},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:yuan,id:1000},  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name:pepe,id:-30}  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}  
&nbsp;&nbsp;&nbsp;&nbsp;}  
}]}"  

We can parse it to a TestObject7 instance like :  
`TestObject7 obj = JsonParser.FromJson<TestObject7>(jsonString);`

More examples can be found in the test class. They are Unity dependent to run, but they can easily parsed to a console application also
