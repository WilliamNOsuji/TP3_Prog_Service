import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Score } from '../models/score';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  domain : string = "https://localhost:7165/";

  myScores : Score[] = [];
  publicScores : Score[] = [];

  constructor( public httpClient : HttpClient ) { }

    async getMyScores() : Promise<any> {
      let x = await lastValueFrom(this.httpClient.get<Score[]>(this.domain + "api/Scores/GetMyScores"));
      console.log(x)

      this.myScores = x
    }

    async getPublicScores() : Promise<void> {
      let y = await lastValueFrom(this.httpClient.get<Score[]>(this.domain + "api/Scores/GetPublicScores"))
      console.log(y)

      y.sort((a, b) => b.scoreValue - a.scoreValue);
      this.publicScores = y.slice(0, 10);
    }

    async editScoreToPublic(score : Score) : Promise<void>{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,true)
      let z = await lastValueFrom(this.httpClient.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + score.id, updatedScore))
      console.log(z)
    }

    async editScoreToPrivate(score : Score) : Promise<void>{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,false)
        let z = await lastValueFrom(this.httpClient.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + score.id, updatedScore))
        console.log(z)
    }

    async addNewScore( newScore : Score) : Promise<void> { 
      let response = await lastValueFrom(this.httpClient.post<Score>(this.domain + "api/Scores/PostScore",newScore) );
      console.log("Score sent successfully:", response);
    } 
}
