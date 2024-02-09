import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
//here the code is written in typescript
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

//OnInit 
export class AppComponent implements OnInit {
  // for declaring an variable in typesccript is title:string=initialize
  //typescript is datatype safety
  //here this title is of string typeo only you cant store anything else in it
  title = 'Dating App';
  users: any; //turing off typescript safety
  //here this private ensure that whatever we use is accessible in this class
  constructor(private http:HttpClient){}

  //this help us to initialize anything after the constructor is made
  //this get method return observable
  ngOnInit(): void {
    //the observable once created it wont do anthing so for make it to read our data we need sunscribe 
    //now in this subscribe we can say what we want to do with observable and it return oberver obj which go to api and give result
    //this is like callbacks in Javascript async
    //next is what we do when we get data
    //error is optional 
    // complete is something that we can do when request is completed because http reauest always complete
    this.http.get("https://localhost:5000/api/users").subscribe({
        next:response=>this.users=response,
        error:er=>console.log(er),
        complete:()=>console.log("requestCompleted")
      });
  }
}
