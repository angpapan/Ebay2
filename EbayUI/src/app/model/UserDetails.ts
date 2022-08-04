export class UserDetails {
  "username": string = "";
  "firstName"?: string = undefined;
  "lastName"?: string = undefined;
  "email": string = "";
  "phoneNumber"?: string = undefined;
  "street"?: string = undefined;
  "streetNumber"?: number = undefined;
  "city": string = "";
  "postalCode"?: string = undefined;
  "country": string = "";
  "vatNumber"?: string = undefined;
  "enabled"?: boolean = undefined;
  "sellerRatingsNum": number = 0;
  "sellerRating": number = 0;
  "bidderRatingsNum": number = 0;
  "bidderRating": number = 0;
  "dateCreated": Date;
}
