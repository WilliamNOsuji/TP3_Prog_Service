import { Component, OnInit } from '@angular/core';
import { Score } from '../models/score';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { HttpService } from '../services/http.service';

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

  constructor(public http : HttpClient, public httpRequest : HttpService) { }

  async ngOnInit() {
    this.userIsConnected = sessionStorage.getItem("token") != null;
    //let x = await lastValueFrom(this.http.get<Score[]>(this.domain + "api/Scores/GetMyScores"))
    //console.log(x)
    //this.myScores = x;
    if(this.userIsConnected){
      let x = await this.httpRequest.getMyScores()
      this.myScores = x
    }
    //let y = await lastValueFrom(this.http.get<Score[]>(this.domain + "api/Scores/GetPublicScores"))
    let y = await this.httpRequest.getPublicScores()
    // Take top 10 scores
    this.publicScores = y;
  }

  async changeScoreVisibility(score : Score){

    if(!score.isPublic){
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,true)
      //let z = await lastValueFrom(this.http.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + id, updatedScore))
      await this.httpRequest.editScoreToPublic(updatedScore)
    }
    else{
      let updatedScore = new Score(score.id, score.pseudo, score.date,score.temps,score.scoreValue,false)
      //let z = await lastValueFrom(this.http.put<Score>(this.domain + "api/Scores/ChangeScoreVisibility/" + id, updatedScore))
      await this.httpRequest.editScoreToPrivate(updatedScore)
    }
    // Reload the page after the change is made
    window.location.reload();
  }
}
