import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  
  registerMode=false;
  users:any;
  
  constructor(private http:HttpClient){} 
  ngOnInit(): void {
    this.getUser();
      }


  registerToggle(){
    this.registerMode=!this.registerMode;
  }

  getUser(){
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

  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }
}
