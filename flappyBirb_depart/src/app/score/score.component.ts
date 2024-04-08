import { Component, OnInit } from '@angular/core';
import { Score } from '../models/score';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-score',
  templateUrl: './score.component.html',
  styleUrls: ['./score.component.css']
})
export class ScoreComponent implements OnInit {

  domain : string = "https://localhost:7165/";

  myScores : Score[] = [];
  publicScores : Score[] = [];
  userIsConnected : boolean = false;

  constructor(public http : HttpClient) { }

  async ngOnInit() {

    this.userIsConnected = sessionStorage.getItem("token") != null;

    let httpOptions = {
      headers : new HttpHeaders({
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + this.userIsConnected
      })
    };

    let x = await lastValueFrom(this.http.get<Score[]>(this.domain + "api/Scores/GetMyScores", httpOptions))
    console.log(x)
    this.myScores = x;

    let y = await lastValueFrom(this.http.get<Score[]>(this.domain + "api/Scores/GetPublicScores"))
    console.log(y)
    // Sort scores in descending order by scoreValue
    y.sort((a, b) => b.scoreValue - a.scoreValue);

    // Take top 10 scores
    this.publicScores = y.slice(0, 10);
  }

  async changeScoreVisibility(score : Score){
    let id = score.id;

    this.userIsConnected = sessionStorage.getItem("token") != null;

    let httpOptions = {
      headers : new HttpHeaders({
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + this.userIsConnected
      })
    };
    if(!score.isPublic){
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,true)
      let z = await lastValueFrom(this.http.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + id, updatedScore, httpOptions))
      console.log(z)
    }
    else{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,false)
      let z = await lastValueFrom(this.http.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + id, updatedScore, httpOptions))
      console.log(z)
    }
    
    // Reload the page after the change is made
    window.location.reload();
  }
}
