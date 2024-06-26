import { Component, OnDestroy, OnInit } from '@angular/core';
import { Game } from './gameLogic/game';
import { Score } from '../models/score';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-play',
  templateUrl: './play.component.html',
  styleUrls: ['./play.component.css']
})
export class PlayComponent implements OnInit, OnDestroy{

  domain : string = "https://localhost:7165/";
  token? : string | null = null

  game : Game | null = null;
  scoreSent : boolean = false;

  scores : Score[] = []
  score ?: Score;

  scorePseudo : string = "";
  scoreDate : string = "";
  scoreTimeValue : string | null = null;
  scoreValue : string | null = null; 
  scoreIsPublic : boolean = false;
  errorMessage: string | null = null;


  constructor(public http : HttpClient, public httpRequest : HttpService){}

  ngOnDestroy(): void {
    // Ceci est crotté mais ne le retirez pas sinon le jeu bug.
    location.reload();
  }

  ngOnInit() {
    this.game = new Game();
  }

  replay(){
    if(this.game == null) return;
    this.game.prepareGame();
    this.scoreSent = false;
  }

  async sendScore() : Promise<void>{
    if(this.scoreSent) return;

    this.scoreSent = true;
    
    // ██ Appeler une requête pour envoyer le score du joueur ██
    // Le score est dans sessionStorage.getItem("score")
    // Le temps est dans sessionStorage.getItem("time")
    // La date sera choisie par le serveur
    this.scoreTimeValue = sessionStorage.getItem("time");
    this.scoreValue = sessionStorage.getItem("score");

      if(this.scoreTimeValue != null && this.scoreValue != null){
        let newScore = new Score(
          0,
          this.scorePseudo,
          this.scoreDate,
          parseFloat(this.scoreTimeValue),
          parseInt(this.scoreValue),
          false
        );
        console.log(newScore)
        try {
          //let response = await lastValueFrom(this.http.post<Score>(this.domain + "api/Scores/PostScore",newScore) );
          await this.httpRequest.addNewScore(newScore);
        } catch (error) {
          // Handle error here
          console.error("User is not Connected !: ", error);
          this.errorMessage = "Error sending score: " + error; // Set error message
        }
      }
      
  }
}
