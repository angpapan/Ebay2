export class ItemDetailsFull {
  "itemId": number;
  "name": string;
  "buyPrice"?: number;
  "price": number;
  "firstBid": number;
  "location": string;
  "country"?: string;
  "description": string;
  "started"?: Date;
  "ends": Date;
  "latitude": number;
  "longitude": number;
  "sellerId": number;
  "sellerName": string;
  "categories"?: string [];
  "images"?: {
    "data": "string"
  }[];
  "bids"?:{
        "bidder": {
          "userId": "string",
          "location": "string",
          "country": "string",
          "rating": number
        },
        "amount": number,
        "time": Date
  }[];
  completed:number ;
  ended:Date;
  buyer:string;

  boughtInstant():boolean{
    return this.buyPrice !== null && this.buyPrice !== undefined && this.buyPrice == this.price
  }

  timePassed():boolean{
    return this.ends < new Date()
  }

  maxBid(): any{
    let b = this.bids?.sort((a, b) => {
      if (a.amount > b.amount)
        return 1;
      return -1;
    });
    return b!==undefined?b[0]:null;
  }



}
