import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
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
  constructor(private accountService: AccountService){}

  //this help us to initialize anything after the constructor is made
  //this get method return observable
  ngOnInit(): void {
    this.setCurrentUser();
  }
 
  setCurrentUser(){
    const userString=localStorage.getItem('user');
    if(!userString)return;
    const user:User=JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }



}
