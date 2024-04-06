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
  }

  async changeScoreVisibility(score : Score){


  }
}
