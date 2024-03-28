import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterDTO } from '../models/registerDTO';
import { lastValueFrom } from 'rxjs';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { publishFacade } from '@angular/compiler';
import { LoginDTO } from '../models/loginDTO';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})



export class LoginComponent implements OnInit {
  domain : string = "https://localhost:4200/";
  hide = true;

  registerUsername : string = "";
  registerEmail : string = "";
  registerPassword : string = "";
  registerPasswordConfirm : string = "";

  loginUsername : string = "";
  loginPassword : string = "";

  constructor(public route : Router, public http : HttpClient) { }

  ngOnInit() {
  }

  async login() : Promise<void>{
    let loginDTO = new LoginDTO(
      this.loginUsername,
      this.loginPassword
    )
    let x = await lastValueFrom(this.http.post<LoginDTO>( this.domain + "api/Users/Login", loginDTO));
    console.log(x)

    // Redirection si la connexion a réussi :
    this.route.navigate(["/play"]);
  }

  async register() : Promise<void>{
    let registerDTO = new RegisterDTO(
      this.registerUsername,
      this.registerEmail,
      this.registerPassword,
      this.registerPasswordConfirm
    );
    let x = await lastValueFrom(this.http.post<RegisterDTO>( this.domain + "api/Users/Register", registerDTO));
    console.log(x)
  }
}
