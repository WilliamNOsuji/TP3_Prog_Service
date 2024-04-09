import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterDTO } from '../models/registerDTO';
import { lastValueFrom } from 'rxjs';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { publishFacade } from '@angular/compiler';
import { LoginDTO } from '../models/loginDTO';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})



export class LoginComponent implements OnInit {
  domain : string = "https://localhost:7165/";
  hide = true;
 
  registerUsername : string = "";
  registerEmail : string = "";
  registerPassword : string = "";
  registerPasswordConfirm : string = "";

  loginUsername : string = "";
  loginPassword : string = "";

  constructor(public route : Router, public http : HttpClient, public httpRequest : HttpService) { }

  ngOnInit() {
  }

  async login() : Promise<void>{
    let loginDTO = new LoginDTO(
      this.loginUsername,
      this.loginPassword
    )
    //const response = await lastValueFrom(this.http.post<any>(this.domain + "api/Users/Login", loginDTO));
    //console.log(response)
    await this.httpRequest.loginIn(loginDTO);

    // Store token in session storage
    console.log("User has Logged In Successfully !!!")
    
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
    //let x = await lastValueFrom(this.http.post<RegisterDTO>( this.domain + "api/Users/Register", registerDTO));
    await this.httpRequest.register(registerDTO);
    console.log("User has been Registered Successfully !!!")
  }
}
