export class Score{

    constructor(
        public id : number,
        public pseudo : string | null,
        public date : string | null,
        public temps : number,
        public scoreValue : number,
        public isPublic : boolean
    ){}

}