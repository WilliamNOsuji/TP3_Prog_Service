import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Score } from '../models/score';
import { lastValueFrom } from 'rxjs';
import { RegisterDTO } from '../models/registerDTO';
import { LoginDTO } from '../models/loginDTO';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  domain : string = "https://localhost:7165/";

  myScores : Score[] = [];
  publicScores : Score[] = [];

  constructor( public httpClient : HttpClient ) { }

    async getMyScores() : Promise<Score[]> {
      let x = await lastValueFrom(this.httpClient.get<Score[]>(this.domain + "api/Scores/GetMyScores"));
      console.log(x)
      return x
    }

    async getPublicScores() : Promise<Score[]> {
      let y = await lastValueFrom(this.httpClient.get<Score[]>(this.domain + "api/Scores/GetPublicScores"))
      console.log(y)
      return y;
    }

    async editScoreToPublic(score : Score) : Promise<Score>{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,true)
      let z = await lastValueFrom(this.httpClient.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + score.id, updatedScore))
      console.log(z)
      return z
    }

    async editScoreToPrivate(score : Score) : Promise<Score>{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,false)
        let z = await lastValueFrom(this.httpClient.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + score.id, updatedScore))
        console.log(z)
        return z
    }

    async addNewScore( newScore : Score) : Promise<any> { 
      let response = await lastValueFrom(this.httpClient.post<Score>(this.domain + "api/Scores/PostScore",newScore) );
      console.log("Score sent successfully:", response);
      return response;
    } 

    async loginIn(loginDTO : LoginDTO) : Promise<void>{
      const response = await lastValueFrom(this.httpClient.post<any>(this.domain + "api/Users/Login", loginDTO));
      console.log(response)
      sessionStorage.setItem('token', response.token);
      console.log("This is the assigned Token (Without the Expiration DateTime): " + sessionStorage.getItem("token")?.toString())
    }

    async register(registerDTO : RegisterDTO) : Promise<void>{
      let x = await lastValueFrom(this.httpClient.post<RegisterDTO>( this.domain + "api/Users/Register", registerDTO));
      console.log(x)
    }
}
