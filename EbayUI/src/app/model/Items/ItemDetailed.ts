export class ItemDetails {
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
  "images"?: string[];
}
