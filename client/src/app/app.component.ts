import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Kiran Dating App';
  users:any;

  constructor(private http:HttpClient){}

  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/Users").subscribe({
      next : Response =>this.users=Response,
      error: error=>console.log(error),
      complete:()=>console.log("Completed")     
      
    })
  }
}
